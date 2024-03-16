const productQuantityInputSelector = "input.product-quantity";
const minusButtonId = "minus-button";
const plusButtonId = "plus-button";

function EditQuantity(event) {
    let button = event.target;
    let isMinus = button.id == minusButtonId;
    let isPlus = button.id == plusButtonId;

    if (!isPlus && !isMinus) {
        console.log("HEllo");
        return;
    }

    const quantityInput = document.querySelector(productQuantityInputSelector);
    let currentQuantity = quantityInput.value;
    let isMinValue = currentQuantity <= 1;

    if (isPlus) {
        currentQuantity = currentQuantity * 1 + 1;
    }
    if (isMinus && !isMinValue) {
        currentQuantity = currentQuantity * 1 - 1;
    }

    quantityInput.value = currentQuantity;
}
