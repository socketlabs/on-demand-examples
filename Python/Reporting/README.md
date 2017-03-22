SocketLabs Python
=======================

Python module for accessing the SocketLabs API

## Installation:

```
pip install socketlabs
```

## Quickstart:

```
>>> from socketlabs import SocketLabs
>>> username = <username>
>>> password = <password>
>>> serverid = <serverid>
>>> socketlabs = SocketLabs(username=username, password=password,
                            serverid=serverid)
>>> failed = socketlabs.failedMessages()
```
