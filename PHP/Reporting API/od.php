<?php

//prints each recipient email addresses associated for delivery failures
//
//replace the following constant **** values with your own
define("SERVER_ID", "9999");
define("API_USER", "username");
define("API_PASSWORD", "YOUR API KEY HERE");

//calls messagesFailed
$service_url = 'https://api.socketlabs.com/v1/messagesFailed?serverId=' . SERVER_ID . '&type=xml';
$curl = curl_init($service_url);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
curl_setopt($curl, CURLOPT_USERPWD, API_USER . ':' . API_PASSWORD);
$curl_response = curl_exec($curl);
curl_close($curl);

//parses XML response and outputs the email addresses of associated with each failure
$xml = new SimpleXMLElement($curl_response);
echo $xml->count[0] . ' records returned:<br/>';

echo $xml->collection->item[0]->ToAddress;
foreach($xml->collection->item as $item) {
	echo $item->ToAddress . '<br/>';
}
print_r($xml);	

//see https://www.socketlabs.com/od/api for complete API documentation

?>