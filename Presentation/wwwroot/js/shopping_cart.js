// Api endpoints section.
const add_item_to_cart_endpoint = "https://localhost:7233/api/cart/add";
const remove_item_from_cart_endpoint = "https://localhost:7233/api/cart/remove";

// Query selectors section.
// Products section.
const productNameSelector = ".product-name";
const productQuantitySelector = ".product-quantity";
const productImageSelector = "img[name='product-image']";
const unitPriceSelector = "[name='unit-price']";

// Shopping Cart section.
const cartItemsCountSelector = "#cart-items-count";
const cartTotalSelector = "#cart-total";
const cartItemListSelector = "ul#item-list";
const cartItemSelector = "[name='cart-item']";
const cartItemQuantitySelector = ".cart-item-quantity";
const cartItemUnitPriceSelector = ".cart-item-unit-price";

// HtmlElements.
const body = document.querySelector("body");

const shoppingCart = document.querySelector("#view-shopping-cart");
const cartItemList = document.querySelector(cartItemListSelector);
const cartItemsCount = document.querySelector(cartItemsCountSelector);
const cartTotal = document.querySelector(cartTotalSelector);

function InitAddToCartRequest(productId, quantity) {
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

function ReloadShoppingCart() {
	const cartItems = cartItemList.querySelectorAll(cartItemSelector);

	// Reload the items count.
	cartItemsCount.innerText = cartItems.length;

	let total = 0;

	for (let item of cartItems) {
		let quantity = item.querySelector(cartItemQuantitySelector);
		let unitPrice = item.querySelector(cartItemUnitPriceSelector);

		let subTotal = quantity.innerText * 1 * unitPrice.innerText;
		total += subTotal;
	}

	cartTotal.innerText = total;
}

ReloadShoppingCart();

function InitRemoveItemFromCartRequest(productId) {
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
	let cartItems = cartItemList.querySelectorAll(cartItemSelector);

	for (let item of cartItems) {
		let isFound = item.id == productId;

		if (isFound) {
			let cartItemQuantity = item.querySelector(cartItemQuantitySelector);
			let newQuantity = cartItemQuantity.innerText*1 + quantity*1;

			cartItemQuantity.innerText = newQuantity;

			ReloadShoppingCart(true);
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

	cartItemList.innerHTML += html;

	ReloadShoppingCart(true);
}

async function AddItemToCart(event) {
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

		const request = InitAddToCartRequest(productId, quantity);
		const response = await fetch(request);

		if (!response.ok) {
			return;
		}

		CreateCartItemHtml(productId, productName, quantity, unitPrice, imageUrl);
		event.target.disabled = false;
	}
}

async function RemoveItemFromCart(event) {
	const removeItemButtonName = "remove-item-button";
	const product = event.currentTarget;
	const productId = product.id;

	if (event.target.id == removeItemButtonName) {
		const request = InitRemoveItemFromCartRequest(productId);
		const response = await fetch(request);

		if (!response.ok) {
			return;
		}

		// Remove the cart item and reload the cart.
		product.parentNode.removeChild(product);
		ReloadShoppingCart(true);
	}
}

function HideBodyScrollBar() {
	body.style = "overflow: hidden;";
}

function ShowBodyScrollbar() {
	body.style = "";
	body.removeAttribute("style");
}