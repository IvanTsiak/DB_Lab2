db.products.find({
    "category": "Smartphones",
    "attributes.color": "White",
    "attributes.memory": "256GB"
}).limit(3);