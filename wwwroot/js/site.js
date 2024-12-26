var matchdrop=document.querySelector('#dropdown1');
if(matchdrop){
  matchdrop.addEventListener('change', function () {
    var tournamentId = this.value;
    var matchDropdown2 = document.querySelector('#dropdown2');

    if (!tournamentId) {
      matchDropdown2.innerHTML = '<option value="" selected>Select a match</option>';
      return;
    }

    fetch(`${window.location.origin}/GetMatches?tournamentId=${tournamentId}`)
      .then(response => {
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
      })
      .then(data => {
        console.log('Fetched matches:', data); // Inspect response data
        matchDropdown2.innerHTML = '<option value="" selected>Select a match</option>'; // Reset dropdown

        if (data.length > 0) {
          data.forEach(match => {
            const option = document.createElement('option');
            option.value = match.id;
            option.textContent = `${match.id} - ${match.name}`;
            matchDropdown2.appendChild(option);
          });
        } else {
          const option = document.createElement('option');
          option.value = "";
          option.textContent = "No matches found";
          matchDropdown2.appendChild(option);
        }
      })
      .catch(err => {
        console.error('Error fetching matches:', err);
      });
  });
}

var confirmNavigateButton = document.querySelector('#confirmNavigatetodataEnter');
if (confirmNavigateButton) {
  confirmNavigateButton.addEventListener('click', function () {
    var tournamentId = document.querySelector('#dropdown1').value;
    var matchElement = document.querySelector('#dropdown2');
    var matchNo = matchElement.value;
    var matchName = matchElement.options[matchElement.selectedIndex]?.text || ""; // Get the match name (text of the option)

    if (!tournamentId || !matchNo) {
      alert('Please select both a tournament and a match.');
      return;
    }

    var url = `/dataEnter?tournamentId=${encodeURIComponent(tournamentId)}&matchNo=${encodeURIComponent(matchNo)}&matchName=${encodeURIComponent(matchName)}`;
    window.location.href = url;
  });
}

var confirmNavigatetoScoring = document.querySelector('#confirmNavigatetoScoring');
if (confirmNavigatetoScoring) {
  confirmNavigatetoScoring.addEventListener('click', function () {
    // Redirect to /Scoring page
    var url = '/Scoring';
    window.location.href = url;
  });
}



document.addEventListener("DOMContentLoaded", function () {
    const updateButton = document.getElementById("save");
    if (updateButton) {
      updateButton.addEventListener("click", function () {
        const middleBox = document.querySelector(".middle-box");
        if (middleBox) {
          middleBox.style.display = "none";
        }
      });
    }
  });
