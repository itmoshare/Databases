import random
import codecs
import csv

def genRoutes():
    with open('neo4j_data_routes.csv', "w") as output:
        writer = csv.writer(output, lineterminator='\n')
        for i in range(20000):
            writer.writerow([i])
    return

def genStops():
    with open('neo4j_data_stops.csv', "w") as output:
        writer = csv.writer(output, lineterminator='\n')
        for i in range(200000):
            writer.writerow([i])
    return 

def genTimings():
    with open('neo4j_data_timings.csv', "w") as output:
        writer = csv.writer(output, lineterminator='\n')
        for i in range(10000):
            day = random.randint(1,7)
            hour = random.randint(0,23)
            hourString = str(hour).zfill(2)
            minute = random.randint(0,59)
            minuteString = str(minute).zfill(2)
            writer.writerow(['{}:{}'.format(hourString,minuteString),day,i])
    return

def genLinks():
    with open('neo4j_data_links.csv', "w") as output:
        writer = csv.writer(output, lineterminator='\n')
        for i in range(20000):
            for j in range(20):
                timing = random.randint(0,10000)
                stop = random.randint(0,200000)
                writer.writerow([i,timing,stop])
    return
genRoutes()
genStops()
genTimings()
genLinks()