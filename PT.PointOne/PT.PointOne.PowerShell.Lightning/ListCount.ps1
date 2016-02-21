#
# ListCount.ps1
#
$url = "https://aspc1606.sharepoint.com/sites/dev"
Connect-SPOnline $url -Credentials "Aspc1606"
$libraryName = "LargeLibrary"
$list = get-spolist $libraryName
$list.ItemCount