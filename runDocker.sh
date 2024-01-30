#!/bin/bash
# have to execute from its folder because appsetings won't get loaded properly for some reason...
cd /gspAPI
touch error.log
dotnet gspAPI.dll 2> error.log&
P1=$!
touch server.log
touch error.log
node /app/server.js >> /app/server.log 2> /app/error.log &
P2=$!
wait $P1 $P2
