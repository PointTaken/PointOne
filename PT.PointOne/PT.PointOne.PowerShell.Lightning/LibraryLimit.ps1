#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/large -Credentials "Aspc1606"

$libraryName = "Documents"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Tant","Fjas")
$folderNames | Foreach { 
    $folderName = $_ 
}
