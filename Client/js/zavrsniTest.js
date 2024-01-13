// podaci od interesa
var host = "https://localhost:";
var port = "44388/";
var telefoniEndpoint = "api/telefoni/";
var proizvodjaciEndpoint = "api/proizvodjaci/";
var loginEndpoint = "api/authentication/login";
var registerEndpoint = "api/authentication/register";
var pretragaEndpoint = "pretraga"
var formAction = "Create";
var editingId;
var jwt_token;

function loadPage() {
	loadTelefoni();
}

// prikaz telefona
function loadTelefoni() {
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "none";

	// ucitavanje telefona
	var requestUrl = host + port + telefoniEndpoint;
	console.log("URL zahteva: " + requestUrl);
	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}
	console.log(headers);
	fetch(requestUrl, { headers: headers })
		.then((response) => {
			if (response.status === 200) {
				
				response.json().then(setTelefoni);
			} else {
				console.log("Error occured with code " + response.status);
				showError();
			}
		})
		.catch(error => console.log(error));
}

// metoda za postavljanje telefona u tabelu
function setTelefoni(data) {
	var container = document.getElementById("data");
	container.innerHTML = "";

	console.log(data);

	// ispis naslova
	var div = document.createElement("div");
	var h1 = document.createElement("h1");
    var headingText = document.createTextNode("Telefoni");
    h1.style.textAlign = "center";
	h1.appendChild(headingText);
	div.appendChild(h1);

	// ispis tabele
	var table = document.createElement("table");
	table.className = "table table-bordered table-hover";

	var header = createHeader();
	table.append(header);

	var tableBody = document.createElement("tbody");

	for (var i = 0; i < data.length; i++) {
		// prikazujemo novi red u tabeli
		var row = document.createElement("tr");
		// prikaz podataka
		row.appendChild(createTableCell(data[i].proizvodjac.naziv));
        row.appendChild(createTableCell(data[i].model));
        row.appendChild(createTableCell(data[i].cena));
        row.appendChild(createTableCell(data[i].dostupnaKolicina));
        

		if (jwt_token) {
			row.appendChild(createTableCell(data[i].operativniSistem));

			// prikaz dugmeta za brisanje
			var stringId = data[i].id.toString();

			var buttonDelete = document.createElement("button");
			buttonDelete.name = stringId;
			buttonDelete.addEventListener("click", deleteTelefon);
			buttonDelete.className = "btn btn-danger";
			var buttonDeleteText = document.createTextNode("Delete");
			buttonDelete.appendChild(buttonDeleteText);
			var buttonDeleteCell = document.createElement("td");
			buttonDeleteCell.appendChild(buttonDelete);
			row.appendChild(buttonDeleteCell);
		}
		tableBody.appendChild(row);
	}

	table.appendChild(tableBody);
	div.appendChild(table);

	// prikaz forme
	if (jwt_token) {
		document.getElementById("formDiv").style.display = "block";	
	}
	// ispis novog sadrzaja
	container.appendChild(div);
}

// brisanje telefona
function deleteTelefon() {
	// izvlacimo {id}
	var deleteID = this.name;
	// saljemo zahtev 
	var url = host + port + telefoniEndpoint + deleteID.toString();
	var headers = { 'Content-Type': 'application/json' };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { method: "DELETE", headers: headers})
		.then((response) => {
			if (response.status === 204) {
				console.log("Successful action");
				refreshTable();
			} else {
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
};

function showError() {
	var container = document.getElementById("data");
	container.innerHTML = "";

	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var errorText = document.createTextNode("Error occured while retrieving data!");

	h1.appendChild(errorText);
	div.appendChild(h1);
	container.append(div);
}

function createHeader() {
	var thead = document.createElement("thead");
	thead.classList.add("table-header");
	var row = document.createElement("tr");
	
	row.appendChild(createTableHeaderCell("Proizvodjac"));
    row.appendChild(createTableHeaderCell("Model"));
    row.appendChild(createTableHeaderCell("Cena (din)"));
    row.appendChild(createTableHeaderCell("Kolicina"));

	if (jwt_token) {
		row.appendChild(createTableHeaderCell("OS"));
		row.appendChild(createTableHeaderCell("Akcija"));
		
	}

	thead.appendChild(row);
	return thead;
}

function createTableHeaderCell(text) {
	var cell = document.createElement("th");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}

function createTableCell(text) {
	var cell = document.createElement("td");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}


// prikaz forme za registraciju
function showRegistration() {
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "block";
    document.getElementById("logout").style.display = "none";
	document.getElementById("pocetnaForma").style.display = "none";
	document.getElementById("formDivPretraga").style.display = "none";
}

function registerUser() {
	var username = document.getElementById("usernameRegister").value;
	var email = document.getElementById("emailRegister").value;
	var password = document.getElementById("passwordRegister").value;
	var confirmPassword = document.getElementById("confirmPasswordRegister").value;

	if (validateRegisterForm(username, email, password, confirmPassword)) {
		var url = host + port + registerEndpoint;
		var sendData = { "Username": username, "Email": email, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					document.getElementById("registerForm").reset();
					console.log("Successful registration");
					alert("Successful registration");
					showLogin();
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Greska prilikom registracije!");
					response.text().then(text => { console.log(text); })
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function validateRegisterForm(username, email, password, confirmPassword) {
    
    
    if (username.length === 0) {
		alert("Username field can not be empty.");
		return false;
	} else if (email.length === 0) {
		alert("Email field can not be empty.");
		return false;
	} else if (password.length === 0) {
		alert("Password field can not be empty.");
		return false;
	} else if (confirmPassword.length === 0) {
		alert("Confirm password field can not be empty.");
		return false;
	} else if (password !== confirmPassword) {
		alert("Password value and confirm password value should match.");
		return false;
	}
	return true;
}

// prikaz forme za prijavu
function showLogin() {
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "block";
	document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("logout").style.display = "none";
	document.getElementById("pocetnaForma").style.display = "none";
	document.getElementById("formDivPretraga").style.display = "none";
}

function loginUser() {
	var username = document.getElementById("usernameLogin").value;
	var password = document.getElementById("passwordLogin").value;

	if (validateLoginForm(username, password)) {
		var url = host + port + loginEndpoint;
		var sendData = { "Username": username, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					document.getElementById("loginForm").reset();
					console.log("Successful login");
					alert("Successful login");
					response.json().then(function (data) {
						console.log(data);
						document.getElementById("info").innerHTML = "Prijavljeni korisnik: <i>" + data.username + "<i/>.";
						document.getElementById("logout").style.display = "block";
						document.getElementById("pocetnaForma").style.display = "none";
						document.getElementById("formDivPretraga").style.display = "block";

						jwt_token = data.token;
						loadTelefoni();
						loadProizvodjaciForDropdown();

					});
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Greska prilikom prijave!");
					response.text().then(text => { console.log(text); })
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function validateLoginForm(username, password) {
	if (username.length === 0) {
		alert("Username field can not be empty.");
		return false;
	} else if (password.length === 0) {
		alert("Password field can not be empty.");
		return false;
	}
	return true;
}

function cancelLogging(){
    document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("logout").style.display = "none";
	document.getElementById("pocetnaForma").style.display = "block";
	document.getElementById("formDivPretraga").style.display = "none";
    
}

function cancelRegistration(){
        document.getElementById("formDiv").style.display = "none";   
        document.getElementById("loginFormDiv").style.display = "none";
        document.getElementById("registerFormDiv").style.display = "none";
        document.getElementById("logout").style.display = "none";
        document.getElementById("pocetnaForma").style.display = "block";
        document.getElementById("formDivPretraga").style.display = "none";
    
    }

    function logout() {
        jwt_token = undefined;
        document.getElementById("info").innerHTML = "";
        document.getElementById("data").innerHTML = "";
        document.getElementById("formDiv").style.display = "none";
        document.getElementById("loginFormDiv").style.display = "block";
        document.getElementById("registerFormDiv").style.display = "none";
        document.getElementById("logout").style.display = "none";
        document.getElementById("pocetnaForma").style.display = "initial";
        loadTelefoni();
    }

    function submitPretragaForm() {
        event.preventDefault();
    
        if (!jwt_token) {
            alert("Morate biti prijavljeni da biste izvrsili pretragu.");
            return;
        }
    
        var najmanjaCena = parseFloat(document.getElementById("najmanjaCena").value);
        var najvecaCena = parseFloat(document.getElementById("najvecaCena").value);

    
        if (isNaN(najmanjaCena) || isNaN(najvecaCena) || najmanjaCena > najvecaCena ) {
            alert('Unesite ispravan unos cena.');
            return;
        }
      
        document.getElementById("data").style.display = "block";

        var data = {
            Najmanje: najmanjaCena,
            Najvise: najvecaCena
        };
    
        var url = host + port + telefoniEndpoint + pretragaEndpoint;
    
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + jwt_token 
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(data => {
            console.log('Odgovor sa servera:', data);
            prikaziRezultatePretrage(data);
        })
        .catch(error => {
            console.error('Greška:', error);
        });
    }

    function prikaziRezultatePretrage(data) {
        var rezultatiDiv = document.getElementById("data");
        rezultatiDiv.innerHTML = "";
    
        // Dodaj naslov
        var h1 = document.createElement("h1");
        h1.textContent = "Telefoni";
        h1.style.textAlign = "center";
        rezultatiDiv.appendChild(h1);
    
        
        var table = document.createElement("table");
        table.className = "table table-bordered table-hover";
    
        var header = createHeader(); 
    
        table.append(header);
    
        var tableBody = document.createElement("tbody");
    
        for (var i = 0; i < data.length; i++) {
            var row = document.createElement("tr");
    
            // Dodajte ćelije sa podacima koji vam trebaju
            row.appendChild(createTableCell(data[i].proizvodjacNaziv));
            row.appendChild(createTableCell(data[i].model));
            row.appendChild(createTableCell(data[i].cena));
            row.appendChild(createTableCell(data[i].dostupnaKolicina));
    
            // Dodajte dugme za brisanje ako je potrebno
            if (jwt_token) {
                row.appendChild(createTableCell(data[i].operativniSistem));
              
                // prikaz dugmadi za izmenu i brisanje
                var stringId = data[i].id.toString();
    
                var buttonDelete = document.createElement("button");
                buttonDelete.name = stringId;
                buttonDelete.addEventListener("click", deleteTelefoniNovaTabela);
                buttonDelete.className = "btn btn-danger";
                var buttonDeleteText = document.createTextNode("Delete");
                buttonDelete.appendChild(buttonDeleteText);
                var buttonDeleteCell = document.createElement("td");
                buttonDeleteCell.appendChild(buttonDelete);
                row.appendChild(buttonDeleteCell);
                
            }
    
            tableBody.appendChild(row);
        }
    
        table.appendChild(tableBody);
        rezultatiDiv.appendChild(table);
    }

    function deleteTelefoniNovaTabela() {
        // izvlacimo {id}
        var deleteID = this.name;
        // saljemo zahtev 
        var url = host + port + telefoniEndpoint + deleteID.toString();
        var headers = { 'Content-Type': 'application/json' };
        if (jwt_token) {
            headers.Authorization = 'Bearer ' + jwt_token;
        }
    
        fetch(url, { method: "DELETE", headers: headers})
            .then((response) => {
                if (response.status === 204) {
                    console.log("Successful action");
                    refreshTable();
                   
                } else {
                    console.log("Error occured with code " + response.status);
                    alert("Error occured!");
                }
            })
            .catch(error => console.log(error));
    };

    function submitTelefonForm() {
        console.log("Submit button clicked");
        event.preventDefault(); // Sprečava podnošenje obrasca i ponovno učitavanje stranice
    
        var model = document.getElementById("model").value;
        var os = document.getElementById("OS").value;
        var kolicina = parseInt(document.getElementById("kolicina").value);
        var cena = parseFloat(document.getElementById("cena").value);
        var proizvodjacId = parseInt(document.getElementById("proizvodjacId").value);
        
        var httpAction;
        var sendData;
        var url;
        
        if (model.length<3 || model.length>120 ) {
            alert('Unos telefona od 3 do 120 karaktera.');
            return;
        }
        if (os.length<2 || os.length>30 ) {
            alert('Unos OS od 2 do 20 karaktera.');
            return;
        }
        if (kolicina<0 || kolicina>1000 ) {
            alert('Dostupna kolicina od 0 do 1000');
            return;
        }
        if (cena<1 || cena>250000 ) {
            alert('Cena stana od 1 do 250000');
            return;
        }
        
        // u zavisnosti od akcije pripremam objekat
        if (formAction === "Create") {
            httpAction = "POST";
            url = host + port + telefoniEndpoint;
     
           sendData = {
                "Model": model,
                "OperativniSistem": os,
                "DostupnaKolicina": kolicina,
                "Cena": cena,
                "ProizvodjacId": proizvodjacId
            };
        }
        else {
            httpAction = "PUT";
            url = host + port + telefoniEndpoint + editingId.toString();
            sendData = {
                "Id": editingId,
                "Model": model,
                "OperativniSistem": os,
                "DostupnaKolicina": kolicina,
                "Cena": cena,
                "ProizvodjacId": proizvodjacId
            };
        }
    
        console.log("Objekat za slanje");
        console.log(sendData);
        var headers = { 'Content-Type': 'application/json' };
        if (jwt_token) {
            headers.Authorization = 'Bearer ' + jwt_token;
        }
        fetch(url, { method: httpAction, headers: headers, body: JSON.stringify(sendData) })
            .then((response) => {
    
                if (response.status === 200 || response.status === 201) {
                    console.log("Successful action");
                    formAction = "Create";
                    refreshTable();
                } else {
                    console.log("Error occured with code " + response.status);

                    response.text().then(text => console.log("Error response text:", text));
                    alert("Error occured!");
                   
                }
            })
            
            .catch(error => console.log(error));
        return false;
    }

  // osvezi prikaz tabele
    function refreshTable() {
	// cistim formu
	document.getElementById("telefonForm").reset();
	// osvezavam
	loadTelefoni();
        };
    
    function loadProizvodjaciForDropdown() {
        // ucitavanje prodavnica
        var requestUrl = host + port + proizvodjaciEndpoint;
        console.log("URL zahteva: " + requestUrl);
    
        var headers = {};
        if (jwt_token) {
            headers.Authorization = 'Bearer ' + jwt_token;
        }
    
        fetch(requestUrl, {headers: headers})
            .then((response) => {
                if (response.status === 200) {
                    response.json().then(setProizvodjaciInDropdown);
                } else {
                    console.log("Error occured with code " + response.status);
                }
            })
            .catch(error => console.log(error));
    };

    function setProizvodjaciInDropdown(data) {
        var dropdown = document.getElementById("proizvodjacId");
        dropdown.innerHTML = "";
        for (var i = 0; i < data.length; i++) {
            var option = document.createElement("option");
            option.value = data[i].id;
            var text = document.createTextNode(data[i].naziv);
            option.appendChild(text);
            dropdown.appendChild(option);
        }
    }
    