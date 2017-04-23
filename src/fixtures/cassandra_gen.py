import random
from urllib.parse import urlencode
from urllib.request import Request, urlopen
import json
import codecs

def getBusStops():
    url = 'http://transport.orgp.spb.ru/Portal/transport/stops/list'
    fields = { 
        'sEcho':'2',
        'iColumns':'7',
        'sColumns':'id,transportType,name,images,nearestStreets,routes,lonLat',
        'iDisplayStart':'0',
        'iDisplayLength':'1000',
        'sNames':'id,transportType,name,images,nearestStreets,routes,lonLat',
        'iSortingCols':'1',
        'iSortCol_0':'0',
        'sSortDir_0':'asc',
        'bSortable_0':'true',
        'bSortable_1':'true',
        'bSortable_2':'true',
        'bSortable_3':'false',
        'bSortable_4':'true',
        'bSortable_5':'false',
        'bSortable_6':'false',
        'transport-type':'0'
    }
    request = Request(url, urlencode(fields).encode())
    jsonString = urlopen(request).read().decode()
    response = json.loads(jsonString)
    stops = response['aaData']
    stopNames = [a[2] for a in stops]
    stopLongs = [a[6]['lon'] for a in stops]
    stopLats  = [a[6]['lat'] for a in stops]
    stopsData = []
    for i in range(len(stopNames)):
        stopsData.append({'name':stopNames[i],'lon':stopLongs[i],'lat':stopLats[i]})
    return stopsData

script = """CREATE KEYSPACE db WITH REPLICATION = {'class':'SimpleStrategy', 'replication_factory':1}";
use db;
CREATE TABLE bus_stops (
    id int PRIMARY KEY,
    title text,
    longitude double,
    latitude double
);
BEGIN BATCH
"""
stops = getBusStops()
for i in range(1000000):
    index = random.randint(0, len(stops) - 1)
    script += 'INSERT INTO bus_stops (id,tittle,longtitude,latitude) values ({},\'{}\',{},{});'.format(i,stops[index]['name'],stops[index]['lon'],stops[index]['lat'])
script+='APPLY BATCH;'
with codecs.open('cassandra_data.cql','w','utf-16') as fp:
    fp.write(script)
    fp.close()

