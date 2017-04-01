def updown(x):
	for i in range(1, x): yield i
	for i in range(x, 1, -1): yield i


for i in updown(5):
	print (i)
