var matchdrop=document.querySelector('#dropdown1');
if(matchdrop){
  matchdrop.addEventListener('change', function () {
    var tournamentId = this.value;
    var matchDropdown2 = document.querySelector('#dropdown2');

    matchDropdown2.innerHTML=''

    fetch(`${window.location.origin}/GetMatches?tournamentId=${tournamentId}`)
      .then(response => {
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
      })
      .then(data => {
        if (data.length > 0) {
          data.forEach(match => {
            const option = document.createElement('option');
            // option.value=match.matchid;
            option.value=JSON.stringify({ matchid: match.matchid, MatchNo: match.id });
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
    matchValue=matchElement.value;
    // var matchName = matchElement.options[matchElement.selectedIndex]?.text || ""; 

    var matchData;
    try {
      matchData = JSON.parse(matchValue);
    } catch (e) {
      console.error('Error parsing match value:', e);
      alert('Invalid match data. Please select again.');
      return;
    }

    var matchid = matchData.matchid;
    var MatchNo = matchData.MatchNo;

    if (!matchid || !MatchNo) {
      alert('Incomplete match data. Please select a valid match.');
      return;
    }


    if (!tournamentId || !matchid) {
      alert('Please select both a tournament and a match.');
      return;
    }

    var url = `/dataEnter?tournamentId=${encodeURIComponent(tournamentId)}&matchId=${encodeURIComponent(matchid)}&MatchNo=${encodeURIComponent(MatchNo)}`;
    window.location.href = url;
  });
}



document.addEventListener("DOMContentLoaded", function () {
    var updateButton = document.getElementById("save");
    if (updateButton) {
      updateButton.addEventListener("click", function () {
        const middleBox = document.querySelector(".middle-box");
        if (middleBox) {
          middleBox.style.display = "none";
        }
      });
    }
  });


document.addEventListener("DOMContentLoaded", function () {
    var updateButton = document.getElementById("toggleButton");
    if (updateButton) {
      updateButton.addEventListener("click", function () {
        const middleBox = document.querySelector(".middle-box");
        if (middleBox) {
          middleBox.style.display = "flex";
        }
      });
    }
  });

  function validateForm() {
    // var teamselection=document.getElementById("");
    var homePlayerNames = document.getElementsByName("homePlayerName[]");
    var homeBatchNos = document.getElementsByName("homeBatchNo[]");
    var homePlayerStatuses = document.getElementsByName("homePlayerStatus[]");

    if (homePlayerNames.length !== homeBatchNos.length || homePlayerNames.length !== homePlayerStatuses.length) {
        alert("All home team lists must have the same number of elements.");
        return false;
    }

    return true;
}

document.querySelectorAll('.data-form').forEach(select => {
  select.addEventListener('change', function () {
      this.value = this.value.trim();
  });
});


function restoreBatchSelection() {
  const savedBatch = localStorage.getItem('batchNo');
  if (savedBatch) {
    const batchNoDropdown = document.getElementById('batchNoDropdown');
    batchNoDropdown.value = savedBatch;
    console.log('Restored batch number from localStorage:', savedBatch);
    fetchBatchPlayers();
  }
}

document.addEventListener('DOMContentLoaded', () => {
  // Restore batch selection from localStorage
  restoreBatchSelection();

  // Add event listener to save batch selection
  const batchNoDropdown = document.getElementById('batchNoDropdown');
  batchNoDropdown.addEventListener('change', () => {
    localStorage.setItem('batchNo', batchNoDropdown.value);
    console.log('Saved batch number to localStorage:', batchNoDropdown.value);

    fetchBatchPlayers();
  });
});

  ////retaining data for inning and turn
  function saveDropdownValues() {
    var inningValue = document.getElementById("inndropdown").value;
    var turnValue = document.getElementById("turndropdown").value;

    localStorage.setItem("inning", inningValue);
    localStorage.setItem("turn", turnValue);
}

function loadDropdownValues() {
    var savedInning = localStorage.getItem("inning");
    var savedTurn = localStorage.getItem("turn");

    if (savedInning) {
        document.getElementById("inndropdown").value = savedInning;
    }
    if (savedTurn) {
        document.getElementById("turndropdown").value = savedTurn;
    }
}

document.getElementById("inndropdown").addEventListener("change", saveDropdownValues);
document.getElementById("turndropdown").addEventListener("change", saveDropdownValues);

window.addEventListener("load", loadDropdownValues);


//after clicking the clear button to clear local storage
document.getElementById('clearStorageBtn').addEventListener('click', () => {
  localStorage.clear();  // Clears all data in localStorage
  console.log('Local storage cleared!');
  
  // Optionally, reset the batch dropdown to the default value
  const batchNoDropdown = document.getElementById('batchNoDropdown');
  batchNoDropdown.value = '';
  console.log('Batch dropdown reset to default value.');
  
  // Optionally, clear any UI elements related to saved selections
  const tableBody = document.querySelector('.grid-container tbody');
  tableBody.innerHTML = '';  // Clear the table data

  ///inn turn data clear
  const turnData=document.getElementById("inndropdown");
  turnData.value='';
  const innData=document.getElementById("turndropdown");
  innData.value= '';

});


//page onload clearing all data
window.onload = function() {
  localStorage.clear();  // Clears all data in localStorage
  console.log('Local storage cleared on page load!');
  
  // Optionally, reset the batch dropdown to the default value
  const batchNoDropdown = document.getElementById('batchNoDropdown');
  if (batchNoDropdown) batchNoDropdown.value = '';
  
  // Optionally, clear any UI elements related to saved selections
  const tableBody = document.querySelector('.grid-container tbody');
  if (tableBody) tableBody.innerHTML = '';  // Clear the table data

  // Clear turn and inning data
  const turnData = document.getElementById("inndropdown");
  if (turnData) turnData.value = '';
  const innData = document.getElementById("turndropdown");
  if (innData) innData.value = '';
}


//showing data save popup
function showPopup(message) {
  var popup = document.getElementById('popup');
  popup.textContent = message;  

  popup.classList.add('show');

  setTimeout(function() {
      popup.classList.remove('show');
  }, 3000); 
}

/////////////////////////saving data
function fetchBatchPlayers() {
  var urlParams = new URLSearchParams(window.location.search);
  const matchId = document.getElementById('MatchId').value;
  const batchNo = document.getElementById('batchNoDropdown').value;
  var tournamentId = urlParams.get('tournamentId');
  const isHomeAttacking = parseInt(document.getElementById('isHomeAttacking').value, 10); 
  const isAwayAttacking = parseInt(document.getElementById('isAwayAttacking').value, 10); 
  var inning = document.getElementById('inndropdown').value;
  var turn = document.getElementById('turndropdown').value;
  var attackingTeamId=document.getElementById('attackid').getAttribute('data-teamid');
  var defendingTeamId=document.getElementById('defendid').getAttribute('data-teamid');

  console.log('Match ID:', matchId);
  console.log('Batch Number:', batchNo);
  console.log('Is Home Team Attacking:', isHomeAttacking);
  console.log('Is Away Team Attacking:', isAwayAttacking);
  console.log('Tournament ID:', tournamentId);
  console.log("inning",inning);
  console.log("turn",turn);
  console.log("attackingTeamId",attackingTeamId);
  console.log("defendingTeamId",defendingTeamId);

  const attackingList = isHomeAttacking === 1 ? '#inPlayList' : '#playersList';
  const defendingList = isHomeAttacking === 1 ? '#playersList' : '#inPlayList';

  fetch(`/Scoring/FilterPlayersByBatch?batchNo=${batchNo}&matchId=${matchId}&isHomeAttacking=${isHomeAttacking}&isAwayAttacking=${isAwayAttacking}&tournamentId=${tournamentId}`, {
    method: 'GET'
  })
    .then(response => {
      console.log('Response Status:', response.status);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then(data => {
      console.log('Fetched Data:', data);

      const attackingPlayerButtons = document.querySelectorAll(`${attackingList} button`);
      const attackingPlayers = Array.from(attackingPlayerButtons).map(button => ({
        id: button.value,
        name: button.textContent.trim()
      }));

      const tableBody = document.querySelector('.grid-container tbody');
      tableBody.innerHTML = ''; 

      if (data.players && data.players.length > 0) {
        console.log('Players found:', data.players);

        data.players.forEach(player => {
          const row = document.createElement('tr');

          // Out Time Cell
          const timeCell = document.createElement('td');
          const timeInput = document.createElement('input');
          timeInput.type = 'text'; 
          timeInput.id = `time_${player.playerid}`; 
          timeInput.name = 'fname';
          //timeInput.value = '00';
          timeInput.value = player.TurnTimer || '00';
          timeCell.appendChild(timeInput);

          // Save Button Cell
          const saveCell = document.createElement('td');
          const saveButton = document.createElement('button');
          saveButton.className = 'finalSave';
          saveButton.id = `fbtn_${player.playerid}`; 
          saveButton.textContent = 'Save';
          saveCell.appendChild(saveButton);

          // Attacker Dropdown
          const attackerCell = document.createElement('td');
          const attackerDropdown = document.createElement('select');
          attackerDropdown.className = 'data-form';
          attackerDropdown.required = true;

          const defaultAttackingOption = document.createElement('option');
          defaultAttackingOption.value = '';
          defaultAttackingOption.selected = true;
          defaultAttackingOption.textContent = 'Select Attacker';
          attackerDropdown.appendChild(defaultAttackingOption);

          attackingPlayers.forEach(attacker => {
            const option = document.createElement('option');
            option.value = attacker.id;
            option.textContent = attacker.name;
            attackerDropdown.appendChild(option);
          });

          // Restore attacker selection from localStorage if available
          const selectedAttackerId = localStorage.getItem(`attacker_${player.playerid}`);
          if (selectedAttackerId) {
            console.log(`Restoring attacker selection for player ${player.playerid}: ${selectedAttackerId}`);
            attackerDropdown.value = selectedAttackerId;
          }

          attackerDropdown.dataset.playerId = player.playerid;
          attackerCell.appendChild(attackerDropdown);

          // Defender Button
          const defenderCell = document.createElement('td');
          const defenderButton = document.createElement('button');
          defenderButton.className = 'grid';
          defenderButton.setAttribute('data-player-id', player.playerid);
          defenderButton.textContent = player.playername;
          defenderButton.value=player.playerid;

          // Restore defender selection from localStorage if available
          const selectedDefenderName = localStorage.getItem(`defender_${player.playerid}`);
          if (selectedDefenderName) {
            console.log(`Restoring defender selection for player ${player.playerid}: ${selectedDefenderName}`);
            defenderButton.textContent = selectedDefenderName;
          }

          defenderButton.dataset.playerId = player.playerid;
          defenderCell.appendChild(defenderButton);

          // Stats Dropdown
          const statsCell = document.createElement('td');
          const statsDropdown = document.createElement('select');
          statsDropdown.className = 'data-stats';
          statsDropdown.required = true;

          const statsOptions = [
            { value: '', text: 'STATS' },
            { value: '1', text: 'RUNNING TOUCH' },
            { value: '6', text: 'SELF OUT' },
            { value: '2', text: 'POLE DIVE' },
            { value: '5', text: 'SKY DIVE' },
          ];

          statsOptions.forEach(optionData => {
            const option = document.createElement('option');
            option.value = optionData.value;
            option.textContent = optionData.text;
            statsDropdown.appendChild(option);
          });

          // Restore stats selection from localStorage
          const savedStats = localStorage.getItem(`stats_${player.playerid}`);
          if (savedStats) {
            console.log(`Restoring stats selection for player ${player.playerid}: ${savedStats}`);
            statsDropdown.value = savedStats;
          }

          statsCell.appendChild(statsDropdown);

          // Append all cells to the row
          row.appendChild(timeCell);
          row.appendChild(attackerCell);
          row.appendChild(defenderCell);
          row.appendChild(statsCell);
          row.appendChild(saveCell);

          // Append the row to the table body
          tableBody.appendChild(row);

          // Event Listeners to save data
          attackerDropdown.addEventListener('change', () => {
            console.log(`Saving attacker selection for player ${player.playerid}: ${attackerDropdown.value}`);
            localStorage.setItem(`attacker_${player.playerid}`, attackerDropdown.value);
          });

          defenderButton.addEventListener('click', () => {
            console.log(`Saving defender selection for player ${player.playerid}: ${defenderButton.textContent}`);
            localStorage.setItem(`defender_${player.playerid}`, defenderButton.textContent);
          });

          statsDropdown.addEventListener('change', () => {
            console.log(`Saving stats selection for player ${player.playerid}: ${statsDropdown.value}`);
            localStorage.setItem(`stats_${player.playerid}`, statsDropdown.value);
          });

          // Save button click event
          saveButton.addEventListener('click', () => {
            const time = document.getElementById(`time_${player.playerid}`).value.trim();
            const attacker = attackerDropdown.value.trim();
            const defender = defenderButton.value.trim();
            const stats = statsDropdown.value.trim();

            const playerId = player.playerid.trim();
            const batchNoValue = batchNo.trim();
            const inningNo=inning;
            const turnNo=turn;
            const attackerid=attackingTeamId.trim();
            const defenderid=defendingTeamId.trim();


            // Save or send data here
            const playerData = {
              time: time,
              attacker: attacker,
              defender: defender,
              stats: stats,
              playerId: playerId,
              matchId: matchId,
              batchNo: batchNoValue,
              Inning: inningNo,
              Turn: turnNo,
              attackingTeamId: attackerid,
              defendingTeamId: defenderid,
            };

            console.log('Saving player data:', playerData);


           fetch('/Scoring/SavePlayerData', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify(playerData)
              // body: JSON.stringify({
              //   time: time || 0,
              //   attacker: attacker || "0",
              //   defender: defender || "0",
              //   stats: stats || "0",
              //   playerId: playerId || "0",
              //   matchId: matchId || "0",
              //   batchNo: batchNoValue || "0",
              // })
            })
            
            .then(response=>{
              if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
              }
    return response.text();
    })

    .then(text => {
      console.log('Response text:', text);
      const json = JSON.parse(text || '{}'); 
      console.log('Parsed JSON:', json);
      showPopup("Data is saved");
    })

  .catch(error => {
              console.error('Error saving player data:', error);
            });

            localStorage.setItem(`player_${player.playerid}_data`, JSON.stringify(playerData));
          });
        });
      } else {
        console.log('No players in play');
        tableBody.innerHTML = '<tr><td colspan="4">No players in play</td></tr>';
      }
    })
    .catch(error => console.error('Error fetching players:', error));
}



document.getElementById('undo').addEventListener('click', async () => {
  try {
      const response = await fetch('/Scoring/UndoLastInningBatch', {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
      });

      const result = await response.json();
      if (result.success) {
          alert('Last operation undone successfully.');
      } else {
          alert('Undo operation failed: ' + result.message);
      }
  } catch (error) {
      alert('Error: ' + error.message);
  }
});