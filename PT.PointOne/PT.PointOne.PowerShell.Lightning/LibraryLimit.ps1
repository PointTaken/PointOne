#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/large -Credentials "Aspc1606"
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

# test that we can add a single file
Add-SPOFile -Path "C:\Temp\Document.docx" -Folder "$libraryName/Test" 

