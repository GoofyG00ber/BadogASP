// Fetch and display all products
    let tbody = document.getElementById("productsTableBody");

    // Always fetch and populate the table
    fetch("api/product")
        .then(response => response.json())
        .then(products => {
            console.log("GET:", products);

            //tbody.innerHTML = ""; // Clear any existing content before populating

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
    })
        .then(response => {
            console.log("API Response:", response); // Log the raw response
            if (response.ok) {
                alert("Product added successfully!");

                // Dynamically add the new product to the table
                const tbody = document.getElementById("productsTableBody");
                let row = document.createElement("tr");
                row.innerHTML = `
                <td>Temporary</td> <!-- Use actual ID if your API returns it -->
                <td>${productData.name}</td>
                <td>${productData.description}</td>
                <td>${productData.price}</td>
                <td>${productData.discountPrice}</td>
                <td>${productData.categoryId}</td>
                <td>${productData.stockQuantity}</td>
                <td>${productData.imageUrl}</td>
                <td>${new Date().toISOString()}</td>
                <td>${new Date().toISOString()}</td>
            `;
                tbody.appendChild(row);

                // Clear input fields after submission
                document.getElementById("name").value = "";
                document.getElementById("description").value = "";
                document.getElementById("price").value = "";
                document.getElementById("discountPrice").value = "";
                document.getElementById("categoryId").value = "";
                document.getElementById("stockQuantity").value = "";
                document.getElementById("imageUrl").value = "";
            } else {
                response.text().then(error => {
                    console.error("API Error:", error);
                    alert("Error adding product: " + error);
                });
            }
        })
        .catch(error => {
            console.error("Fetch Error:", error);
            alert("An unexpected error occurred: " + error.message);
        });
});

