class OrderViewModel {
    htmlTag = null;
    constructor(htmlTag) {
        this.htmlTag = htmlTag;
        this.htmlTag.textContent = this.getBasket()?.length;
    }

    addToBasket(orderProduct) {
        const orders = this.getBasket();

        if (orders != null && orders.length > 0) {
            sessionStorage.setItem('basket', JSON.stringify([orderProduct, ...orders]));
        }
        else {
            sessionStorage.setItem('basket', JSON.stringify([orderProduct]));
        }

        this.htmlTag.textContent = this.getBasket()?.length;
    }

    getBasket() {
        const json = sessionStorage.getItem('basket');

        if (json === undefined) {
            return [];
        }

        return JSON.parse(json);
    }

    removeFromBasket(index) {
        let orders = this.getBasket();

        if (orders == null) {
            return orders;
        }

        if (orders.length == 1) {
            orders = this.cleanBasket();
        }
        else if (orders.length > 1) {
            orders.splice(index, 1);
            sessionStorage.setItem('basket', JSON.stringify(orders));

            this.htmlTag.textContent = this.getBasket()?.length;

        }
    }

    cleanBasket() {
        sessionStorage.removeItem('basket');

        this.htmlTag.textContent = '';
    }
}

function fill(basket) {
    const root = document.getElementById('ordered-items');

    if (root?.children.length > 0) {
        root.replaceChildren()
    }

    if (!basket) {
        basket = [];
    }

    const template = document.getElementById("ordered-item");

    const countDiv = document.querySelectorAll('.count-div');

    countDiv[0].textContent = `${basket.length} items`
    countDiv[1].textContent = `ITEMS ${basket.length}`

    let totalPrice = 0

    basket.forEach(x => {
        const newItem = template.content.cloneNode(true);
        const tags = newItem.querySelectorAll('.js-fill');

        tags[0].src = x.Product.image;
        tags[1].textContent = x.Product.name;
        tags[2].textContent = x.Product.model;
        tags[3].textContent = `${x.Product.price} BGN    *    ${x.Count}`;

        totalPrice += x.Product.price * x.Count;
        root.appendChild(newItem);
    });

    const totalPriceDiv = document.getElementById('total-price');
    totalPriceDiv.textContent = `${totalPrice} BGN`
}

let orderViewModel = null;

document.addEventListener("DOMContentLoaded", function () {
    let el = document.getElementById('basket');
    orderViewModel = new OrderViewModel(el);
});
