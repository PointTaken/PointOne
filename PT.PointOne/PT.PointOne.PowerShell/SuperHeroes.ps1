#
# SuperHeroes.ps1
#
$url = http://aspc1606.sharepoint.com/sites/PointOneArms

#Connect-SPOnline -Url $url -Credentials Aspc1606

#$web = Get-SPOWeb

$heroes = Get-Content "C:\Projects\PointOne\PT.PointOne\Resources\Superheroes.txt"
$heroes | % {
	$_
}
