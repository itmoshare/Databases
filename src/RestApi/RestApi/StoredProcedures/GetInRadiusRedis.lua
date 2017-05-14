local x = ARGV[1]
local y = ARGV[2]
local r = ARGV[3]

local res = {}
local j = 1
for i=1,1,+1
do
	local stringCoors = redis.call('GET', i)
	local longString, latString = stringCoors:match("(%S+)%s+(.+)")
	local long = tonumber(longString)
	local lat = tonumber(latString)
	local dToCenter = math.sqrt((lon-x)^2+(lat-y)^2)
	if dToCenter < r then 
		res[j]=i
		j=j+1
		end
return res
