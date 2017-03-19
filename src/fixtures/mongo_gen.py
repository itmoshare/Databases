import random2 # learn more: https://python.org/pypi/random2

def rand_char():
  return chr(random2.choice(list(range(ord('a'), ord('z')))))


# Generate route
def gen_route(d):
  d['route'] = str(random2.randint(1, 1000))
  # Add letter to route
  if random2.randint(0, 10) == 0:
    doc['route'] = doc['route'] + rand_char()

# Generate vehicle number
def gen_vehicle_number(d):
  d['number'] = '{}{}{}{}{}{}'.format(rand_char(), random2.randint(0, 9), random2.randint(0, 9), random2.randint(0, 9), rand_char(), rand_char())

fixtures = {
  'bus' : [gen_route, gen_vehicle_number],
  'subway train' : [gen_route],
  'repairing machine' : [gen_vehicle_number]
}

res = []
for unitId in range(1, 5):
  tp = random2.choice(list(fixtures.keys()))
  doc = {
    'id' : unitId,
    'type' : tp
  }
  
  for f in fixtures[tp]:
    f(doc)
  
  res.append(doc)

print(res)