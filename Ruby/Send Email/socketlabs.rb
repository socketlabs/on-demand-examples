require 'rubygems'
require 'json'
require 'net/http'
require 'net/https'

module SocketLabs   
  
  class << self
    attr_accessor :max_attempts, :url, :pw, :user, :ssl
    def max_attempts
      @max_attempts ||= 3
    end
    def ssl
      @ssl || (true)
    end
    def port
      @port || (ssl ? 443 : 80)
    end
    
  end
    
  class EmailMessage
    
    attr_accessor :to, :from, :cc, :bcc, :subject, :template, :textBody, :htmlBody, :messageId, :mailingId, :replyTo, :headers

    def send
      msg = Hash.new
      msg['From']=from
      msg['To']=to
      msg['Cc']=cc
      msg['Bcc']=bcc
      msg['Subject']=subject
      msg['Template']=template
      msg['TextBody']=textBody
      msg['HtmlBody']=htmlBody
      msg['ReplyTo']=replyTo
      msg['Headers'] = headers
      SocketLabs::SendRawHashMessage(msg);
    end

  end
        
  def self.SendQuickMessage(from, to, subject, textBody, options={})
    msg = Hash.new
    msg['From']=from
    msg['To']=to
    msg['Subject']=subject
    msg['TextBody']=textBody
    options.each do | k, v |
      msg['Bcc']=v if k=="bcc"
      msg['Cc']=cc if k=="cc"
      msg['Template']=template if k=="template"
      msg['HtmlBody']=htmlBody if k=="htmlBody"
      msg['ReplyTo']=replyTo if k=="replyTo"
      msg['Headers'] = headers if k=="headers"
    end
    SocketLabs::SendRawHashMessage(msg);  
  end
  
  def self.SendRawHashMessage(msgHash)
    SocketLabs::SendRawJsonMessage(msgHash.to_json)
  end
        
  def self.SendRawJsonMessage(msgJson)
    @attempt = 1
    begin
      api_url = URI.parse(url)
      req = Net::HTTP::Post.new(api_url.path)
      req.basic_auth user, pw
      req.body=msgJson
      req.set_content_type('application/x-www-form-urlencoded')
      req.delete("Accept")
      req.add_field("Accept", "application/json")
      req.add_field("Content-Type", "application/json; charset=utf-8")
      req.add_field("User-Agent", "SocketLabs Ruby Gem")
      http = Net::HTTP.new(api_url.host, @port)
      http.use_ssl= @ssl
      res = http.start {|http| http.request(req) }
    rescue Exception => e
      if @attempt < max_attempts
        @attempt += 1
        retry
      else
        raise
      end
    end  
    res.body
  end
    
end
