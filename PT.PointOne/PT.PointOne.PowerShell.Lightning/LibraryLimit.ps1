#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/large -Credentials "Aspc1606"

# do we have a library?
Get-SPOList "Documents"
