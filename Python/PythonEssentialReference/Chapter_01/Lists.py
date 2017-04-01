names = ['Sumanesh','Saveetha']
names.append('Aadhavan')
names.insert(2,'Aghilan')

print("\nAll names in the family")
for name in names:
    print(name)

print("\nNames starting after index 2")
for name in names[2:]:
    print(name)

names = names + ["Nila"]
print("\n Future extension")
for name in names:
    print(name)
