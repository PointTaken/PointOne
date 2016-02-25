#
# Orders.ps1
#
$url = "https://aspc1606.sharepoint.com/sites/PointOneArms"

Connect-SPOnline -Url $url -Credentials Aspc1606

class Orders {
    $numberOfBeerTypes;
    $listName;
    #$preferredBeers;
	Orders([int]$numberOfBeerTypes) { #, $preferredBeers) {
        $this.listName = "Orders";
		$this.numberOfBeerTypes = $numberOfBeerTypes;
		#$this.preferredBeers = $preferredBeers;
	}
    [string] GetLookupFormat($item) {
	    return "$($item.ID);#$($item['Title'])"
    }
    AddOrder($beer, $amount = 100) {
        $beerFormatted = $this.GetLookupFOrmat($beer)
        $deliveryDate = ([System.DateTime]::Today).AddDays(14)
        Add-SPOListItem -List $this.listName -Values @{ 
		    "Beer" = $beerFormatted;
		    "Amount" = $amount;
            "ETD" = $deliveryDate; } 
    }
    EnsureOrderListExists() {
	    $listTitle = $this.listName
	    $list = Get-SPOList $listTitle 
        $beerList = Get-SPOList "Beers" 
        $beersId = $beerList.ID
	    if (!$list) {
            Write-Host "Creating list"
		    $list = New-SPOList -Title $listTitle -Template GenericList -OnQuickLaunch:$true
            $f = Add-SPOFieldFromXml -List $listTitle "<Field Type='Lookup' DisplayName='Beer' Required='TRUE' Indexed='TRUE' EnforceUniqueValues='TRUE' List='$($beersId)' ShowField='Title' ID='{F3FBDCF7-526A-42FB-B544-BDC002B04986}' StaticName='Beer' Name='Beer' />"
		    $f = Add-SPOField -DisplayName "Amount" -InternalName "Amount" -List $listTitle -Type Integer -AddToDefaultView -Required:$true 
		    $f = Add-SPOField -DisplayName "ETD" -InternalName "ptETD" -List $listTitle -Type DateTime -AddToDefaultView -Required:$false 
            $titleField = Get-SPOField -List $listTitle -Identity "Title"
            $titleField.Hidden = $true
            $titleField.Required = $false
            $titleField.Update()
            Execute-SPOQuery
		    $view = Add-SPOView -List $listTitle -Title "Orders" -Fields "Beer","Amount","ETD" -SetAsDefault:$true
        }
    }
}

Ensure-OrdersExists
$orders = [Orders]::new(100)
$beers = Get-SPOListItem -List "Beers"
$beer = Get-Random -InputObject $beers
$orders.AddOrder($beer, 50)
