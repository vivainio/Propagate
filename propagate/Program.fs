// propagate
open System.IO
open System.Diagnostics

module OsPath = 
    let fileName = Path.GetFileName
    

let doCopy (srcdir: string) (tgtdir: string) = 
    let srcFiles = Directory.GetFiles(srcdir, "*.*") |> Array.filter File.Exists 
    let srcMap = srcFiles |> Array.map (fun e -> (OsPath.fileName e, e)) |> Map.ofArray
    
    let tgtFiles = Directory.GetFileSystemEntries(tgtdir, "*.*", SearchOption.AllDirectories) |> Array.filter File.Exists
    let found = tgtFiles |> Array.choose (fun e ->
            let fname = OsPath.fileName e
            let srcPath = Map.tryFind fname srcMap
            match srcPath with
            | None -> None
            | Some pth when pth = e -> None
            | Some pth -> Some (pth, e)
            )
    let copyGroups = found |> Array.groupBy fst |> Array.map (fun (src, tgts) -> (src, Array.map snd tgts))   

    let reportTargetDirs =
        let targetDirs = found |> Array.map (snd >> Path.GetDirectoryName) |> Set.ofArray
        printfn "Copying to"
        targetDirs |> Set.iter (printfn "  %s")  
        ()

    let copyTo (src: string) (targets: #seq<string>) =
        let tryCopy src tgt =
            try 
                File.Copy(src, tgt, true)
                true
            with
            | :? IOException as e when e.HResult &&& 0xffff = 32 ->
                printfn "INUSE: %s" tgt
                false
            
        seq {
            for tgt in targets do
                yield tryCopy src tgt
        } |> Seq.takeWhile id |> Array.ofSeq |> ignore
            
    let sw = Stopwatch.StartNew()
    for src, tgts in copyGroups do
        copyTo src tgts
    sw.Stop()
    printfn "Copied %d files %d msec" found.Length sw.ElapsedMilliseconds
    ()

[<EntryPoint>]
let main argv =

    match argv with
    | [| srcDir; tgtDir|] ->
        doCopy srcDir tgtDir
        ()
    | _ -> printfn "Usage:\npropagate <SOURCEDIR> <TARGETDIR>" 

    printfn "%A" argv
    0 // return an integer exit code
