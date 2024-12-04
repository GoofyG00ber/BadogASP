// Fetch and display all products
document.getElementById("fetchProductsButton").addEventListener("click", () => {
    let tbody = document.getElementById("productsTableBody");

    if (tbody.innerHTML !== "") {
        // If content exists, clear it to "hide" the table
        tbody.innerHTML = "";
    } else {
        // Otherwise, fetch and populate the table
        fetch("api/product")
            .then(response => response.json())
            .then(products => {
                console.log("GET:", products);

                tbody.innerHTML = ""; // Clear any existing content before populating

                for (let product of products) {
                    let row = document.createElement("tr");
                    row.innerHTML = `
                        <td>${product.productId}</td>
                        <td>${product.name}</td>
                        <td>${product.description}</td>
                        <td>${product.price}</td>
                        <td>${product.discountPrice}</td>
                        <td>${product.categoryId}</td>
                        <td>${product.stockQuantity}</td>
                        <td>${product.imageUrl}</td>
                        <td>${product.createdAt}</td>
                        <td>${product.updatedAt}</td>
                    `;
                    tbody.appendChild(row);
                }
            })
            .catch(error => console.error("Error fetching products:", error));
    }
});

// Add a new product
document.getElementById("addProductButton").addEventListener("click", () => {
    const productData = {
        "name": document.getElementById("name").value,
        "description": document.getElementById("description").value,
        "price": parseFloat(document.getElementById("price").value),
        "discountPrice": parseFloat(document.getElementById("discountPrice").value) || null,
        "categoryId": parseInt(document.getElementById("categoryId").value) || null,
        "stockQuantity": parseInt(document.getElementById("stockQuantity").value),
        "imageUrl": document.getElementById("imageUrl").value
    };

    fetch("api/product", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(productData)
    }).then(response => {
        if (response.ok) {
            alert("Product added successfully!");
            location.reload();
        } else {
            alert("Error adding product.");
        }
    }).catch(error => console.error("Error adding product:", error));
});
