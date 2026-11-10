SELECT p.Id, p.SKU, p.Name
FROM Product_NoSQL_Test p
JOIN Product_Attribute_Value pav1 ON p.Id = pav1.ProductId
JOIN Attribute_Dict ad1 ON pav1.AttributeId = ad1.Id AND ad1.Name = 'Color'
JOIN Product_Attribute_Value pav2 ON p.Id = pav2.ProductId
JOIN Attribute_Dict ad2 ON pav2.AttributeId = ad2.Id AND ad2.Name = 'Memory'
WHERE p.CategoryName = 'Smartphones'
  AND pav1.Value = 'White'
  AND pav2.Value = '256GB';