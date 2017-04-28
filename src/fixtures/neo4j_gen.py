import random
import codecs

def genRoutes():
    script = ""
    for i in range(1000000):
        script += "CREATE(r{}:Route {{id:{}}})\n".format(i,i)
    return script

def genStops():
    script = ""
    for i in range(1000000):
        script += "CREATE(s{}:Stop {{id:{}}})\n".format(i,i)
    return script

def genTimings():
    script = ""
    for i in range(10000):
        day = random.randint(1,7)
        hour = random.randint(0,23)
        hourString = str(hour).zfill(2)
        minute = random.randint(0,59)
        minuteString = str(minute).zfill(2)
        script += "CREATE(t{}:Timing {{time:'{}:{}' day:{}}})\n".format(i,hourString,minuteString,day)
    return script
script = genRoutes() + genStops() + genTimings()

for i in range(1000000):
    for j in range(20):
        timing = random.randint(0,10000)
        stop = random.randint(0,1000000)
        script += "CREATE\n\t(r{})-[:STOPS_AT_TIME]->(t{}),\n\t".format(i,timing)
        script += "(t{})-[:APPLIES_TO_STOP]->(s{})".format(timing,stop)
with open('neo4j_data.txt','w') as fp:
    fp.write(script)
