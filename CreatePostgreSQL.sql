CREATE TABLE Product_NoSQL_Test (
    Id SERIAL PRIMARY KEY,
    SKU VARCHAR(50) NOT NULL,
    Name VARCHAR(200) NOT NULL,
    CategoryName VARCHAR(100),
    Price NUMERIC(18,2)
);

CREATE TABLE Attribute_Dict (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE Product_Attribute_Value (
    Id SERIAL PRIMARY KEY,
    ProductId INT REFERENCES Product_NoSQL_Test(Id),
    AttributeId INT REFERENCES Attribute_Dict(Id),
    Value VARCHAR(200) NOT NULL
);

INSERT INTO Attribute_Dict (Id, Name) VALUES
(1, 'Color'), (2, 'Memory'), (3, 'OS'),
(4, 'RAM'), (5, 'Processor'),
(6, 'Size'), (7, 'Material');

INSERT INTO Product_NoSQL_Test (Id, SKU, Name, CategoryName, Price)
SELECT
    i,
    'SKU-' || i,
    CASE
        WHEN i % 3 = 0 THEN 'Smartphone Model ' || i
        WHEN i % 3 = 1 THEN 'Laptop Model ' || i
        ELSE 'T-Shirt ' || i
    END,
    CASE
        WHEN i % 3 = 0 THEN 'Smartphones'
        WHEN i % 3 = 1 THEN 'Laptops'
        ELSE 'Clothing'
    END,
    (random() * 1000 + 10)::numeric(18,2)
FROM generate_series(1, 100000) AS i;

DO $$
DECLARE
    i INT;
    colors TEXT[] := ARRAY['Black', 'White', 'Silver', 'Blue', 'Red'];
    memories TEXT[] := ARRAY['128GB', '256GB', '512GB', '1TB'];
    sizes TEXT[] := ARRAY['S', 'M', 'L', 'XL'];
BEGIN
    FOR i IN 1..100000 LOOP
        IF i % 3 = 0 THEN
            INSERT INTO Product_Attribute_Value (ProductId, AttributeId, Value) VALUES
            (i, 1, colors[floor(random()*5)+1]),
            (i, 2, memories[floor(random()*4)+1]),
            (i, 3, 'Android');
        ELSIF i % 3 = 1 THEN
            INSERT INTO Product_Attribute_Value (ProductId, AttributeId, Value) VALUES
            (i, 4, memories[floor(random()*4)+1]),
            (i, 5, CASE WHEN random() > 0.5 THEN 'Intel Core i7' ELSE 'Intel Core i5' END);
        ELSE
            INSERT INTO Product_Attribute_Value (ProductId, AttributeId, Value) VALUES
            (i, 1, colors[floor(random()*5)+1]),
            (i, 6, sizes[floor(random()*4)+1]),
            (i, 7, 'Cotton');
        END IF;
    END LOOP;
END $$;