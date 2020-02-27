# Propagate

Use case:

You have binaries in directory SOURCE. You want to copy theme to every place under subtree TARGET where a file with same name exists.

In .NET projects, this can be handy e.g. when you have compiled an own version of a library out of the tree, and want to copy the resulting binaries to your main project tree where several subdirectories have taken their own local copy of a library.

## Installation:

```
$ dotnet tool install -g propagate
```

Usage:

```
USAGE: propagate [--help] [--dry] [--verbose] <string>...

PATHS:

    <string>...           Paths: either <SOURCEDIR> <TARGETDIR> or <GLOBPATTERNS>... <TARGETDIR>

OPTIONS:

    --dry                 Dry run (do not copy anything)
    --verbose             Show more diagnostics
    --help                display this list of options.
```

Currently, SOURCEDIR should contain all the files in flat structure. TARGETDIR is scanned recursively to find all the places to put the files.

SOURCEDIR can be the same as TARGETDIR. This will copy all the files in "root tree" into deeper directories of the project.

```
License: MIT.
Copyright (c) 2017 by Ville M. Vainio <vivainio@gmail.com>
```
