#
# Purchases.ps1
#
$url = https://aspc1606.sharepoint.com/sites/PointOneArms

Connect-SPOnline -Url $url -Credentials Aspc1606


function Is-HappyHour($purchaseTime) {
	$happy = $purchaseTime.Hour -ge 18 -and $purchaseTime.Hour -lt 19;
	return $happy
}

function Get-BeerPrice($beer, $purchaseTime) {
	if (Is-HappyHour $purchaseTime) { # happy hour
		return ($beer["Out"] * 0.5);
	}
	return $beer["Out"]
} 

function Get-RandomPatron() {
	$pIx = Get-Random $patronsSP.Length
	$patronsSP[$pix]
}

function Get-RandomBeer($beersSP2 = $beersSP) {
	$bIx = Get-Random $beersSP2.Length
	$beersSP2[$bix]
}

function Get-RandomDate($weekendOnly) {
	$d = Get-Date -Year 2016 -Month 1 -Day 1 -Hour 18 -Minute 0 # happens to be a Friday
	$d = $d.AddDays(7 * 8) # end of February - i.e. ASPC
	if ($weekendOnly) {
		$weeksPast = Get-Random 12
		$d = $d.AddDays(-7 * $weeksPast)
		$rnd = Get-Random 100
		if ($rnd -gt 55) {  # 45% Saturday
			$d = $d.AddDays(1)
		}
	} else {
		$days = Get-Random (12 * 7)
		$d = $d.AddDays(- $days)
	}
	$h = Get-Random 8 
	$m = Get-Random 60
	$d = $d.AddHours($h)
	$d = $d.AddMinutes($m) 
	return $d
}

function Get-LookupFormat($item) {
	return "$($item.ID);#$($item['Title'])"
}

function Get-Patron($patronName) {
	$patronsSP | ? { $_["Title"] -eq $patronName }
}

function Add-Drink($patronName, $date, $beer, $price) {
	$patronSP = Get-Patron $patronName
	# Write-Host $patronName $date $beer $price
	# Write-Host $patronSP["Title"]
	$patronFormatted = Get-LookupFormat $patronSP
	$beerFormatted = Get-LookupFormat $beer
#	 Write-Host $patronFormatted " " $beerFormatted " " $price " " $date 
	Add-SPOListItem -List "Purchases" -Values @{ 
		"Patron" = $patronFormatted; 
		"Beer" = $beerFormatted;
		"Price" = $price; 
		"Purchased" = $date } 
}

function Delete-Purchases() {
    $purchases = Get-SPOListItem -List "Purchases"
    $purchases.Length
    $count = 0
    $purchases | % { 
        $_.DeleteObject(); 
        if ($count -ge 250) {
            $count = 0
            Write-Host "250 deleted.."
            Execute-SPOQuery
        }
        $count = $count + 1
    }
    Execute-SPOQuery
}

class Patron {
	$name;
	$weekendOnly;
	$preferredBeers;
	[uint16]$weeklyUnits;

	Patron([string] $name, [bool]$weekendOnly, $preferredBeers, $weeklyUnits) {
		$this.name = $name;
		$this.weekendOnly = $weekendOnly;
		$this.preferredBeers = $preferredBeers;
		$this.weeklyUnits = $weeklyUnits;
	}

	DrinkBeer() {
		$date = Get-RandomDate $this.weekendOnly
		$beer = Get-Random -InputObject $this.preferredBeers # Get-RandomBeer $this.preferredBeers
		$price = Get-BeerPrice $beer $date
		Add-Drink $this.name $date $beer $price
		if (Is-HappyHour $date) {
			$date = $date.AddSeconds(5)
			Add-Drink $this.name $date $beer $price
		}
	}
	[uint16] WeeklyUnits() {
		return $this.weeklyUnits;
	}
}

function Get-SomePatrons() {
	$patronsSP = Get-SPOListItem -List "Patrons"
	$pmax = $patronsSP.Length
	if ($pmax -gt 25) { $pmax = 25 } # max 25 patrons
	$counter = 0
	$patrons = @()
	while ($counter -lt $pmax) {
		$p = Get-Random -InputObject $patronsSP
		Write-Host $p["Title"] 		$p["WeeklyUnits"]
		$patrons += $p
	    $counter = $counter + 1
	}
	return $patrons;
}