#
# Breweries.ps1
#
$url = "https://aspc1606.sharepoint.com/sites/PointOneArms"
Connect-SPOnline $url -Credentials Aspc1606

$norBreweries = ("Ringnes","Hansa","Aass","Mack","CB","Borg","E. C. Dahls","Tou","Frydenlund","Lundetangen","Grans","Arendals", "Nøgne Ø","Haandbryggeriet")

$norBreweries | % {
    Add-SPOListItem -List "Breweries" -Values @{ "Title" = $_; }
}
