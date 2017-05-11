import random
from urllib.parse import urlencode
from urllib.request import Request, urlopen
import json
import codecs
import csv

def getBusStops():
    url = 'http://transport.orgp.spb.ru/Portal/transport/stops/list'
    fields = { 
        'sEcho':'2',
        'iColumns':'7',
        'sColumns':'id,transportType,name,images,nearestStreets,routes,lonLat',
        'iDisplayStart':'0',
        'iDisplayLength':'100',
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

stops = getBusStops()
with open('cassandra_data.csv', "w") as output:
    writer = csv.writer(output, lineterminator='\n')
    for i in range(1000000):
        index = random.randint(0, len(stops) - 1)
        writer.writerow([i,stops[index]['lat'],stops[index]['lon'],stops[index]['name']])