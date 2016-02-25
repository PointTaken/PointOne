#
# Stock.ps1
#
$url = "https://aspc1606.sharepoint.com/sites/PointOneArms"

Connect-SPOnline -Url $url -Credentials Aspc1606

Function Ensure-StockExists() {
	$listTitle = "Stock"
	$list = Get-SPOList $listTitle 
    $beerList = Get-SPOList "Beers" 
    $beersId = $beerList.ID
	if (!$list) {
        Write-Host "Creating list"
		$list = New-SPOList -Title $listTitle -Template GenericList -OnQuickLaunch:$true
        Add-SPOFieldFromXml -List $listTitle "<Field Type='Lookup' DisplayName='Beer' Required='TRUE' Indexed='TRUE' EnforceUniqueValues='TRUE' List='$($beersId)' ShowField='Title' ID='{F3FBDCF6-526A-42FB-B544-BDC002B04986}' StaticName='Beer' Name='Beer' />"
		Add-SPOField -DisplayName "Amount" -InternalName "Amount" -List $listTitle -Type Integer -Required:$true 
        $titleField = Get-SPOField -List $listTitle -Identity "Title"
        $titleField.Hidden = $true
        $titleField.Required = $false
        $titleField.Update() # I mean it.
        Execute-SPOQuery     # and now I tell you that I mean it.
		Add-SPOView -List $listTitle -Title "Stock" -Fields "Beer","Amount" -SetAsDefault:$true
    }
}

function Get-LookupFormat($item) {
	return "$($item.ID);#$($item['Title'])"
}

function Add-RandomStock() {
	$listTitle = "Stock"
	$list = Get-SPOList $listTitle 
    $context = Get-SPOContext
    if ($list.ItemCount -gt 0) { #something already exists. Restock.
        $items = $list.GetItems([Microsoft.SharePoint.Client.CamlQuery]::CreateAllItemsQuery())
        $context.Load($items)
        $context.ExecuteQuery()
        foreach ($item in $items) {
            [int]$amount = $item["Amount"]
            if ($amount -lt 100) {
                $item["Amount"] = $amount + 100
                $item.Update()
            }
        }
        Execute-SPOQuery
    } else {
        $availableBeers = Get-SPOListItem -List "Beers" 
        $myBeers = Get-Random -InputObject $availableBeers -Count 10
        $mybeers | % {
            $beerFormatted = Get-LookupFormat $_
            $amount = Get-Random -Minimum 5 -Maximum 85
            Add-SPOListItem -List "Stock" -Values @{ 
		        "Beer" = $beerFormatted;
		        "Amount" = $amount } 
        }
    }
}

Ensure-StockExists
Add-RandomStock
