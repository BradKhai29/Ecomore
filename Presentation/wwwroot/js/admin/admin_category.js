// Api endpoints.
const category_api_endpoint = "/api/category";

// Query selectors section.
const categoryItemListSelector = "#category-items";
const categoryTotalProductSelector = "#category-total-products";
const editCategoryIdInputSelector = "#categoryId";
const editCategoryNameInputSelector = "#categoryName";

// Create Modal section
const createModalSelector = "#createModal";
const createCategoryNameInputSelector = "#createCategoryName";

const createCategoryNameInput = document.querySelector(createCategoryNameInputSelector);

// Edit Modal section
const editModalSelector = "#editModal";
const editButtonSelector = "#editButton";
const confirmButtonSelector = "#confirmButton";

const editModal = document.querySelector(editModalSelector);
const editCategoryIdInput = document.querySelector(editCategoryIdInputSelector);
const editCategoryTotalProducts = editModal.querySelector(categoryTotalProductSelector);
const editCategoryNameInput = document.querySelector(editCategoryNameInputSelector);
const editButton = document.querySelector(editButtonSelector);
const confirmButton = document.querySelector(confirmButtonSelector);

// Html Elements section.
const categoryItemList = document.querySelector(categoryItemListSelector);

// Create category section.
function CloseCreateModal() {
    createCategoryNameInput.value = "";
    $(createModalSelector).modal('hide');
}

function InitCreateCategoryRequest(categoryName) {
    const request = new Request(category_api_endpoint, {
        method: 'POST',
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            name: categoryName
        })
    });

    return request;
}

function InitUpdateCategoryRequest(categoryId, categoryName) {
    const request = new Request(category_api_endpoint, {
        method: 'PATCH',
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            id: categoryId,
            name: categoryName
        })
    });

    return request;
}

function InitDeleteCatgoryRequest(categoryId) {
    const url = `${category_api_endpoint}/${categoryId}`;

    const request = new Request(url, {
        method: 'DELETE'
    });

    return request;
}

async function CreateNewCategory() {
    const newCategoryName = createCategoryNameInput.value;

    const request = InitCreateCategoryRequest(newCategoryName);
    const response = await fetch(request);

    if (!response.ok) {
        return;
    }

    const responsePayload = await response.json();
    const body = responsePayload.body;
    
    const newCategoryId = body.id;
    CreateNewCategoryHtml(newCategoryId, newCategoryName);

    CloseCreateModal();
}

function CreateNewCategoryHtml(categoryId, categoryName) {
    const lastCategoryIndexAttribute = "last_category_index";

    let lastCategoryIndex = categoryItemList.getAttribute(lastCategoryIndexAttribute);
    lastCategoryIndex++;

    const categoryHtml =
        `
            <tr class="category-item" id="${categoryId}" onclick="ShowEditModal(event);">
                <input class="category-total-products" value="0" hidden/>
                <td scope="col" class="text-center">
                    <button class="btn text-light fw-bold">${lastCategoryIndex}</button>
                </td>
                <td scope="col">
                    <div class="d-flex justify-content-between">
                        <p href="#" class="m-0 d-flex fw-bold text-decoration-none text-light" >
                            <span class="category-name my-auto">${categoryName}</span>
                        </p>
                        <div>
                            <a href="#" class="btn btn-primary fw-bold">Details</a>
                            <button data-bs-toggle="modal" data-bs-target="${editModalSelector}" class="btn btn-secondary fw-bold">
                                Edit
                            </button>
                        </div>
                    </div>
                </td>
            </tr>
        `;

    categoryItemList.setAttribute(lastCategoryIndexAttribute, lastCategoryIndex);

    categoryItemList.innerHTML += categoryHtml;
}

// Edit category section.
function ShowEditModal(event) {
    const categoryItemNameSelector = ".category-name";
    const currentCategoryItem = event.currentTarget;

    const categoryId = currentCategoryItem.id;
    const categoryName = currentCategoryItem.querySelector(categoryItemNameSelector);
    const categoryTotalProducts = currentCategoryItem.querySelector(categoryTotalProductSelector);

    // Display the detail on the edit modal.
    editCategoryIdInput.id = categoryId;
    editCategoryNameInput.value = categoryName.innerHTML;
    editCategoryTotalProducts.innerHTML = categoryTotalProducts.getAttribute("value");
}

function TurnOnEditMode() {
    // Hide the edit button and display confirm button.
    const hiddenAttribute = "hidden";
    const disabledAttribute = "disabled";

    editButton.setAttribute(hiddenAttribute, true);
    confirmButton.removeAttribute(hiddenAttribute);

    // Enable again the category name input.
    editCategoryNameInput.removeAttribute(disabledAttribute);
}

function TurnOffEditMode() {
    // Hide the edit button and display confirm button.
    const hiddenAttribute = "hidden";
    const disabledAttribute = "disabled";

    editButton.removeAttribute(hiddenAttribute);
    confirmButton.setAttribute(hiddenAttribute, true);

    // Disable again the category name input.
    editCategoryNameInput.setAttribute(disabledAttribute, true);
}

async function ConfirmEdit() {
    const categoryId = editCategoryIdInput.id;

    const request = InitUpdateCategoryRequest(categoryId, editCategoryNameInput.value);
    const response = await fetch(request);

    if (!response.ok) {
        return;
    }

    // Set new name for the category element.
    const categoryItemSelector = `[id='${categoryId}'].category-item`;

    const categoryItem = categoryItemList.querySelector(categoryItemSelector);
    const categoryNameElement = categoryItem.querySelector(".category-name");

    categoryNameElement.innerHTML = editCategoryNameInput.value;

    $(editModalSelector).modal('hide');
    TurnOffEditMode();
}

function CloseEditModal(event) {
    TurnOffEditMode();
}

// Delete category section.
async function DeleteCategory() {
    const categoryId = editCategoryIdInput.id;

    const request = InitDeleteCatgoryRequest(categoryId);
    const response = await fetch(request);

    if (!response.ok) {
        return;
    }

    const deletedItemSelector = `tr.category-item[id='${categoryId}']`;
    const deletedItem = categoryItemList.querySelector(deletedItemSelector);

    categoryItemList.removeChild(deletedItem);
}
