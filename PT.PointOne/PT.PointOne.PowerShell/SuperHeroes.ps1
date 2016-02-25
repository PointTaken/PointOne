#
# SuperHeroes.ps1
#
$url = https://aspc1606.sharepoint.com/sites/PointOneArms

Connect-SPOnline -Url $url -Credentials Aspc1606

$web = Get-SPOWeb

$heroes = Get-Content "C:\Projects\PointOne\PT.PointOne\Resources\Superheroes.txt"
$heroes | % {
    $units = 4 + (Get-Random 21)
	Add-SPOListItem -List "Patrons" -Values @{ "Title" = $_; "WeeklyUnits" = $units }
}
