import os,base64
from zipfile import ZipFile
def extract_zip(input_zip):
    input_zip=ZipFile(input_zip)
    return {name: input_zip.read(name) for name in input_zip.namelist()}     
zip_name = os.sys.argv[1]
zip_data = extract_zip(zip_name)
total_data = bytearray()
for name,allbytes in zip_data.items(): total_data = total_data + allbytes
print( base64.b64encode(total_data).decode("utf-8"), end='', flush=True )