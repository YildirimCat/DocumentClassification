from flask import Flask, request, jsonify
import pytesseract
from PIL import Image
import spacy

app = Flask(__name__)
nlp = spacy.load("en_core_web_sm")  # Text processing (NER)

@app.route('/classify', methods=['POST'])
def classify_document():
    file = request.files['file']
    if file.filename.endswith('.png') or file.filename.endswith('.jpeg'):
        img = Image.open(file)
        text = pytesseract.image_to_string(img) # OCR procedure
    else:
        text = file.read().decode('utf-8')
    
    doc = nlp(text) # spaCy object
    entities = [(ent.text, ent.label_) for ent in doc.ents]

    return jsonify(entities=entities, text=text)                

if __name__ == '__main__':
    app.run(debug=True)
