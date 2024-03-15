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

function visitShop(shop) {
    if (shop.id == "shop1") {
        var target = document.getElementById("map");
        target.innerHTML = '<iframe class="col-12 rounded-2" src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2933.3856961884076!2d23.327910276515418!3d42.674371615185706!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x40aa85ac6b04a067%3A0x1c03d81bc96a6a96!2z0KTQsNC60YPQu9GC0LXRgiDQv9C-INC80LDRgtC10LzQsNGC0LjQutCwINC4INC40L3RhNC-0YDQvNCw0YLQuNC60LA!5e0!3m2!1sbg!2sbg!4v1710458937950!5m2!1sbg!2sbg" width="600" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>';
    }
    else if (shop.id == "shop2") {
        var target = document.getElementById("map");
        target.innerHTML = '<iframe class="col-12 rounded-2" src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d47297.05154350809!2d24.698186637928597!3d42.19166225760517!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14acd1ae47a04147%3A0xc383d346a147f1cb!2z0J_Qu9C-0LLQtNC40LLRgdC60Lgg0YPQvdC40LLQtdGA0YHQuNGC0LXRgiAi0J_QsNC40YHQuNC5INCl0LjQu9C10L3QtNCw0YDRgdC60Lgi!5e0!3m2!1sbg!2sbg!4v1710459064965!5m2!1sbg!2sbg" width="600" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>';
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
