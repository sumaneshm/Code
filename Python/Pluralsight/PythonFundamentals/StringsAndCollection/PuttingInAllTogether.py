from urllib.request import  urlopen

with urlopen('https://www.sixty-north.com/c/t.txt') as story:
    story_words = []
    for line in story:
        story_lines = line.decode('utf-8').split()
        for word in story_lines:
            story_words.append(word)

print(story_words)

