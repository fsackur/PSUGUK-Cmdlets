---
external help file: Dusty.AdConnectivity.dll-Help.xml
online version: 
schema: 2.0.0
---

# Get-DnsResolver

## SYNOPSIS
Get an object representing a DNS server

## SYNTAX

```
Get-DnsResolver [-DnsServer] <String[]> [<CommonParameters>]
```

## DESCRIPTION
Gets a DNS server, which you can specify by IP address or hostname.

The DNS server object is of type DnsResolver and has Query(hostname) method and Query(hostname, recordtype) methods.

## EXAMPLES

### Example 1
```
PS C:\> $DnsServer = Get-DnsResolver "8.8.8.8"

PS C:\> $DnsServer.Query("redhotpaddocks.com")
```

Gets a DNS server and queries it for the addresses of redhotpaddocks.com

### Example 2
```
PS C:\> $DnsServer = Get-DnsResolver "8.8.8.8"

PS C:\> $DnsServer.Query("skillz4bills.biz", "SOA")
```

Gets a DNS server and queries it for the Start Of Authority record for the delightfully tacky skillz4bills.biz domain

## PARAMETERS

### -DnsServer
The IP address or hostname of a DNS server. If hostname is provided, the default DNS resolution mechanism will be used to resolve it to an IP address

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String[]

## OUTPUTS

### Dusty.ADConnectivity.DnsResolver

## NOTES

## RELATED LINKS

