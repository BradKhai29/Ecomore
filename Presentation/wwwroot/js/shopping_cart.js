// Api endpoints section.
const add_item_to_cart_endpoint = "https://localhost:7233/api/cart/add";
const remove_item_from_cart_endpoint = "https://localhost:7233/api/cart/remove";

// HtmlElements.
const body = document.querySelector("body");
const shoppingCart = document.querySelector("#view-shopping-cart");

// Query selectors section.
const productNameSelector = ".product-name";
const productQuantitySelector = ".product-quantity";
const productImageSelector = "img[name='product-image']";
const unitPriceSelector = "[name='unit-price']";

function CreateAddToCartRequest(productId, quantity) {
	let request = new Request(add_item_to_cart_endpoint, {
		method: 'POST',
		headers: {
			"Content-Type": "application/json"
		},
		body: JSON.stringify({
			productId: `${productId}`,
			quantity: quantity
		})
	});

	return request;
}

function CreateRemoveItemFromCartRequest(productId) {
	const url = `${remove_item_from_cart_endpoint}/${productId}`;

	let request = new Request(url, {
		method: 'DELETE'
	});

	return request;
}
function MoveShoppingCart() {
	window.location.href = '/ShoppingCart/Index';
}
function MoveCheckOut() {
	window.location.href = '/CheckOut/Index';
}

function ShowShoppingCart() {
	shoppingCart.style = "z-index: 999; top: 0; right: 0; left: 0; bottom: 0;";

	HideBodyScrollBar();
}

function HideShoppingCart() {
	shoppingCart.style = "display: none";

	ShowBodyScrollbar();
}

function CreateCartItemHtml(productId, productName, quantity, unitPrice, imageUrl) {
	const itemListSelector = "ul#item-list";
	const cartItemtSelector = "[name='cart-item']";
	const cartItemQuantitySelector = ".cart-item-quantity";
	const cartItemUnitPriceSelector = ".cart-item-unit-price";

	let itemList = shoppingCart.querySelector(itemListSelector);

	let cartItems = itemList.querySelectorAll(cartItemtSelector);

	for (let item of cartItems) {
		let isFound = item.id == productId;

		if (isFound) {
			let cartItemQuantity = item.querySelector(cartItemQuantitySelector);
			let newQuantity = cartItemQuantity.innerText*1 + quantity*1;

			cartItemQuantity.innerText = newQuantity;

			return;
		}
	}

	let html =
		`<li name="cart-item" id="${productId}" onclick="RemoveItemFromCart(event);" class="p-2 d-flex shadow-sm">
            <div class="col-auto d-none d-lg-block">
                <img style="width: 100px; height: 100px" class="rounded"
                src="${imageUrl}" />
            </div>
            <div class="ms-2 d-flex flex-grow-1">
                <div class="my-auto">
                    <strong class="d-inline-block mb-2 text-primary">${productName}</strong>
                    <p class="card-text mb-auto">
						<span class="cart-item-quantity">
							${quantity}
						</span>
						x
						<span class="cart-item-unit-price">
							${unitPrice}
						</span>
					</p>
                </div>
            </div>
            <div class="d-flex align-items-center">
                <button id="remove-item-button" class="btn btn-light border-3 rounded-circle">
                    <i id="remove-item-button" class="fa-solid fa-x"></i>
                </button>
            </div>
        </li>`;
	cartItems
	itemList.innerHTML += html;
}

function AddItemToCart(event) {
	const addToCartButtonName = "add-to-cart";

	let isClickOnAddToCartButton = event.target.name === addToCartButtonName;

	if (isClickOnAddToCartButton) {
		// Disable the add-to-cart button.
		event.target.disabled = true;

		// Get product-element and related data.
		const product = event.currentTarget;

		const productId = product.id;
		const productName = product.querySelector(productNameSelector).innerText;
		const quantity = product.querySelector(productQuantitySelector).value;
		const unitPrice = product.querySelector(unitPriceSelector).innerText;
		const imageUrl = product.querySelector(productImageSelector).src;

		const request = CreateAddToCartRequest(productId, quantity);
		const response = fetch(request);

		let isAddSuccess = false;

		response
			.then(function (res) {
				isAddSuccess = (res.status == 200);
			})
			.then(function () {
				if (isAddSuccess) {
					CreateCartItemHtml(productId, productName, quantity, unitPrice, imageUrl);
				}
			});

		event.target.disabled = false;
	}
}

function RemoveItemFromCart(event) {
	const removeItemButtonName = "remove-item-button";
	const product = event.currentTarget;
	const productId = product.id;

	if (event.target.id == removeItemButtonName) {
		let request = CreateRemoveItemFromCartRequest(productId);

		let response = fetch(request);

		let isRemoveSuccess = false;

		response.then(function (res) {
			// If OK status code.
			isRemoveSuccess = (res.status == 200);

		}).then(function() {
			if (isRemoveSuccess) {
				product.parentNode.removeChild(product);
			}
		});
	}
}

function HideBodyScrollBar() {
	body.style = "overflow: hidden;";
}

function ShowBodyScrollbar() {
	body.style = "";
	body.removeAttribute("style");
}