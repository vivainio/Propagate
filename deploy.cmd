msbuild propagate.sln /p:Configuration=Release
cd propagate\bin
rm -R propagate
rm propagate.zip
mv Release propagate
zip -ur propagate.zip propagate