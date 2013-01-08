#
# SocketLabs HTTP injection API sample for message merge to multiple recipients
#

require 'rubygems'
require 'json'
require 'net/http'
require 'net/https'

#build merge data
merge_data=Hash.new
merge_data[:PerMessage]=[
  [{:Field=>'DeliveryAddress', :Value=>'person1@example.com'}, {:Field=>'FavoriteColor', :Value=>'blue'}],
  [{:Field=>'DeliveryAddress', :Value=>'person2@example.com'}, {:Field=>'FavoriteColor', :Value=>'green'}]]
            
#build mesage in a hash table, include merge data from above
data=Hash.new
data[:ApiKey]="YOUR API KEY HERE"
data[:Messages]=[
                  {
                  :MergeData=>merge_data,
                  :Subject=>'Test', 
                  :To=>[{:EmailAddress=>'%%DeliveryAddress%%'}], 
                  :From=>{:EmailAddress=>'sender@example.com'},
                  :HtmlBody=>'<h1>The HTML portion of the message.</h1><br/>Your favorite color is %%FavoriteColor%%.'
                  }
                ]
puts JSON.pretty_generate(data)

#post to HTTPS
url = URI.parse("https://inject.socketlabs.com/api/v1/email")
puts url
req = Net::HTTP::Post.new(url.path)
req.body=data.to_json  
req.set_content_type('application/json')
http = Net::HTTP.new(url.host, url.port)
http.use_ssl= true;
res = http.start {|http| http.request(req) }

#process result
result_object = JSON.parse(res.body)
puts JSON.pretty_generate(result_object)

if result_object['ErrorCode']=='Success'
  puts 'Successfully sent message.'
elsif result_object['ErrorCode']=='Warning'
  puts 'At least one message was sent, but there are errors with one or more messages, or their recipients.'
  #not shown here, but you can traverse the MessageResults object to get more information on each message that encountered an error or warning
else 
  puts 'Failure Code: ' + result_object['ErrorCode']
end
