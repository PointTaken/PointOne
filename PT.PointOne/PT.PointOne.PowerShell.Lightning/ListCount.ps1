#
# ListCount.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials Aspc1606
Get-SPOList LargeLibrary | % { $_.ItemCount }