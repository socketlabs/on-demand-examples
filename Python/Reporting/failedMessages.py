#!/usr/bin/env python

import json
import os
import sys

from socketlabs import SocketLabs

username = os.environ.get('SOCKETLABS_USERNAME')
password = os.environ.get('SOCKETLABS_PASSWORD')
serverid = os.environ.get('SOCKETLABS_SERVERID')

socketlabs = SocketLabs(username = username, password = password, serverid = serverid)

index = 0

while True:

    failed = socketlabs.failedMessages(index = index)

    if len(failed['collection']) == 0:
        break

    for message in failed['collection']:
        print(json.dumps(message, sort_keys=True, indent=4, separators=(',', ': ')))
        index = index + 1
