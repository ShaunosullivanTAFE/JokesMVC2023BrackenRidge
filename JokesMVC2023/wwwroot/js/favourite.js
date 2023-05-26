window.addEventListener("load", async (e) => {
    updateDDL()
    document.getElementById("createListForm").addEventListener('submit', async (e) => {
        await handleCreateList(e);
    })
    console.log($('#createListForm'));

    document.getElementById('btnShowCreateModal').addEventListener('click', (e) => {
        $('#createListModal').modal('show')
    })


});

async function updateDDL() {
    let result = await fetch('/Favourite/FavouriteListDDL');
    let htmlResult = await result.text();
    document.getElementById('selectContainer').innerHTML = htmlResult;

    let ddlContainer = document.getElementById('selectContainer');
    let ddl = ddlContainer.querySelector('select');
    ddl.addEventListener('change', async (e) => {
        handleDDLChange(e);
    })

}

async function handleDDLChange(e) {
    let selectedOption = e.target.selectedOptions[0]
    sessionStorage.setItem('listID', selectedOption.value)
    sessionStorage.setItem('listName', selectedOption.text)

    await UpdateJokeList();
}

async function UpdateJokeList() {
    let result = await fetch('/Favourite/GetJokesForList?listID=' + sessionStorage.getItem('listID'));
    let htmlResult = await result.text();
    let jokeContainer = document.getElementById('jokeContainer');
    jokeContainer.innerHTML = htmlResult;

    // add the event listener to the buttons
    let buttons = jokeContainer.querySelectorAll('input[class="btn btn-outline-danger"]');
    let buttonArray = Array.from(buttons);

    buttonArray.forEach((value, index) => {
        //console.log(value.dataset.itemid)

        value.addEventListener('click', (e) => {
            removeJoke(value.dataset.itemid);
        })

    });

    for (index in buttonArray) {
        console.log(buttonArray[index]);
    }


}

async function handleCreateList(e) {
    e.preventDefault();

    console.log(e.target["listName"].value);

    let result = await fetch('/Favourite/AddNewList', {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(e.target["listName"].value)
    });

    if (result.ok) {
        await updateDDL();
        $('#createListModal').modal('hide');
    }

}

async function removeJoke(jokeId) {
    let listID = sessionStorage.getItem("listID");

    let favListItem = {
        FavouriteListId: listID,
        JokeId: jokeId
    }

    let result = await fetch('/Favourite/RemoveJokeFromList', {
        method: 'DELETE',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(favListItem)
    });
    if (result.ok) {
        // Update Joke Table

        await UpdateJokeList();
    }

    console.log(favListItem);
}