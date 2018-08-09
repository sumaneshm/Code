# lists can be multiplied by integer
s = [[1, 2], [3, 4]] * 2
print(s)

# list modification
s[0].insert(0, -1)
print("After inserting -1 to s[0]", s[0])

s[0].append(1)
print("After appending 1 to s[0]", s[0])

del s[0]
print("After removing s[0], s=", s)

t = s
s.append([5, 6])

print("After appending to s, t=", t)

t.append([10, 12])
print("After appending to t, s is = ", s)

