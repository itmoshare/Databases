import random # learn more: https://python.org/pypi/random2

f = open('redis_data.txt', 'w')
for i in range(1, 1000000):
  print('SET {} "{} {}"'.format(i, random.random() * 180 - 90, random.random() * 360 - 180), file=f)