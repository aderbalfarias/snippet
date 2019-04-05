from urllib.request import urlopen
from bs4 import BeautifulSoup

# specify the url
quote_page = 'https://github.com/aderbalfarias'

# query the website and return the html to the variable ‘page’
page = urlopen(quote_page)

# parse the html using beautiful soap and store in variable `soup`
# get the index price
soup = BeautifulSoup(page, 'html.parser')

#price_box = soup.find_all('span', attrs={'class':'repo'})

for link in soup.find_all('span', attrs={'class':'repo'}):
    print(link.get('title'))