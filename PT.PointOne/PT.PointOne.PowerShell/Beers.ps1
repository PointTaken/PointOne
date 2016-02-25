#
# Beers.ps1
#
#
# Beers.ps1
#
$ringnes = @("Ringnes Pilsner", 
	"Ringnes Lite", 
	"Ringnes Extra Gold", 
	"Ringnes Julebokk", 
	"Ringnes Julespecial", 
	"Ringnes Juleøl", 
	"Ringnes Juleøl Sterk", 
	"Ringnes Lettøl", 
	"Ringnes Platinum", 
	"Ringnes Polaris Baltisk Porter", 
	"Ringnes Polaris Røykbokk", 
	"Ringnes Skjærgårdspils", 

	"Frydenlund Pilsner", 
	"Frydenlund Fatøl", 
	"Frydenlund Bayer", 
	"Frydenlund Pale Ale", 
	"Frydenlund Juleøl", 
	"Frydenlund Bokkøl", 

	"Carlsberg Pilsner", 

	"Tuborg", 
	"Tuborg Juleøl", 
	"Tuborg Lite", 
	"Tuborg Sommerøl", 

	"Dahls Julebrygg", 
	"Dahls Juleøl", 
	"Dahls Juleøl Sterk", 
	"Dahls Pils", 
	"Nordlands Juleøl", 
	"Nordlandspils", 
	"Arendals Pilsner", 
	"Tou Pilsner", 
	"Lysholmer Double Ice"
)


$url = "https://aspc1606.sharepoint.com/sites/PointOneArms"
Connect-SPOnline $url -Credentials Aspc1606

$brewery = Get-SPOListItem -List "Breweries" | ? { $_["Title"] -eq "Ringnes" }

$ringnes | % {
	$in = 30 + (Get-Random 30) 
    $out = 60 + (Get-Random 60) 

	Add-SPOListItem -List "Beers" -Values @{ "Title" = $_; "Brewery" = "$($brewery.ID);#$($brewery['Title'])"; "In" = $in; "Out" = $out }
}
$update = $true
if ($update) {
#	$aromas = @("Fruity", "Texture", "Floral", "Vegetal", "Spicy", "Heat-induced", "Biological")
# Get-Random -InputObject "Fruity", "Texture", "Floral", "Vegetal", "Spicy", "Heat-induced", "Biological" # but these are actually categories...
	$beersSP = Get-SPOListItem -List "Beers"
	$beersSP | % {
		$colourR = [Convert]::ToString((Get-Random 256), 16) # converts to hex
		$colourG = [Convert]::ToString((Get-Random 256), 16)
		$colourB = [Convert]::ToString((Get-Random 256), 16)
		$_["Colour"] = "#$colourR$colourG$colourB"
		# $_["Appearance"] = $appearance # colour, clarity, lacing, glassware, head/head retention. Give me a break.
		$rnd = Get-Random -InputObject 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1
		$_["Bitterness"] = $rnd
		$rnd = Get-Random -InputObject 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1
		$_["Freshness"] = $rnd
		# $_["Fullness"] = $fullness
		$alcohol = Get-Random 50
		if ($alcohol -le 5) { # 10% chance of alcohol free
			$alcohol = 0
		} else {
#			$alcohol = Get-Random -Minimum 3.0 -Maximum 8.5 
			$alcohol = (2.5 + ($alcohol / 10.0)) / 100.0 
		}
		$_["Alcohol"] = $alcohol
		$_.Update()
	}
	Execute-SPOQuery
}