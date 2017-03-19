import random # learn more: https://python.org/pypi/random
import datetime
import json

def rand_char():
  return chr(random.choice(list(range(ord('a'), ord('z')))))


# Generate route
def gen_route(d):
  d['route'] = str(random.randint(1, 1000))
  # Add letter to route
  if random.randint(0, 10) == 0:
    doc['route'] = doc['route'] + rand_char()

# Generate vehicle number
def gen_vehicle_number(d):
  d['number'] = '{}{}{}{}{}{}'.format(rand_char(), random.randint(0, 9), random.randint(0, 9), random.randint(0, 9), rand_char(), rand_char())

malfunction_list = ['the wheel fell off', 'broken window', 'broken engine', 'broken seat']
def gen_malfunctions(d):
  d['malfunctions'] = {} 
  for m in random.sample(malfunction_list, random.randint(0, len(malfunction_list))):
    d['malfunctions'][m] = datetime.date(random.randint(2000, 2017), random.randint(1, 12), random.randint(1, 26)).isoformat()

fixtures = {
  'bus' : [gen_route, gen_vehicle_number, gen_malfunctions],
  'subway train' : [gen_route, gen_malfunctions],
  'repairing machine' : [gen_vehicle_number, gen_malfunctions]
}

res = []
for unitId in range(1, 1000000):
  tp = random.choice(list(fixtures.keys()))
  doc = {
    '_id' : unitId,
    'type' : tp
  }
  
  for f in fixtures[tp]:
    f(doc)
  
  res.append(doc)

with open('mongo_data.json', 'w') as fp:
  json.dump(res, fp, indent = 2)