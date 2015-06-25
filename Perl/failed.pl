#!/usr/bin/perl -w

use strict;
use JSON;
use LWP::UserAgent;

my $baseurl = 'https://api.socketlabs.com/v1';
my $apihost = 'api.socketlabs.com:443';
my $apirealm = 'SocketLabsMarketingAPI';
my $apiusername = 'YOUR USERNAME HERE';
my $apipassword = 'YOUR PASSWORD HERE';
my $serverid = 'YOUR SERVERID HERE';
my $dataformat = 'json';

my $totalcount;
my $count;
my $content;
my $index=0;

my $ua = LWP::UserAgent->new;
$ua->agent("MyApp/0.1 ");

$ua->credentials(
	"$apihost",
	"$apirealm",
	"$apiusername",
	"$apipassword"
);

do {

	my $url = "${baseurl}/messagesFailed?serverId=${serverid}&type=${dataformat}&index=${index}";

	my $response = $ua->get($url);

	die "Error: ", $response->header('WWW-Authenticate') || 'Error accessing', "\n ", $response->status_line, "\n at $url\n Aborting" unless $response->is_success;

	$content=decode_json($response->content);

	$totalcount=$content->{"totalCount"};
	$count=$content->{"count"};

	for(my $i=0 ; $i < $count; $i++) {

		my $mailingid = $content->{"collection"}[$i]->{"MailingId"};
		my $messageid = $content->{"collection"}[$i]->{"MessageId"};
		my $failuretype = $content->{"collection"}[$i]->{"FailureType"}; # 0=soft/temporary, 1=hard/permanent, 2=suppressed
		my $failurecode = $content->{"collection"}[$i]->{"FailureCode"};
		my $reason = $content->{"collection"}[$i]->{"Reason"};
		my $toaddress = $content->{"collection"}[$i]->{"ToAddress"};

		print "Processing record $toaddress\n";

		$index++;

	}

} while ($index<$totalcount);

exit;

