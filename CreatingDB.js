db = db.getSiblingDB('storedb_nosql');

db.products.drop();

let docs = [];
const colors = ['Black', 'White', 'Silver', 'Blue', 'Red'];
const memories = ['128GB', '256GB', '512GB', '1TB'];
const sizes = ['S', 'M', 'L', 'XL'];

for (let i = 1; i <= 100000; i++) {
    let categoryType = i % 3;
    let product = {
        sku: 'SKU-' + i,
        price: Math.floor(Math.random() * 1000) + 10,
        attributes: {}
    };

    if (categoryType === 0) {
        product.name = 'Smartphone Model ' + i;
        product.category = 'Smartphones';
        product.attributes.color = colors[Math.floor(Math.random() * colors.length)];
        product.attributes.memory = memories[Math.floor(Math.random() * memories.length)];
        product.attributes.os = 'Android';
    } else if (categoryType === 1) {
        product.name = 'Laptop Model ' + i;
        product.category = 'Laptops';
        product.attributes.ram = memories[Math.floor(Math.random() * memories.length)];
        product.attributes.processor = 'Intel Core i' + (Math.random() > 0.5 ? '7' : '5');
    } else {
        product.name = 'T-Shirt ' + i;
        product.category = 'Clothing';
        product.attributes.color = colors[Math.floor(Math.random() * colors.length)];
        product.attributes.size = sizes[Math.floor(Math.random() * sizes.length)];
        product.attributes.material = 'Cotton';
    }

    docs.push(product);

    if (i % 2000 === 0) {
        db.products.insertMany(docs);
        docs = [];
    }
}

db.products.countDocuments();