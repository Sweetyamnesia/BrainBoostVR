// ===== Smooth Scroll for Navigation =====
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
  anchor.addEventListener('click', function(e){
    e.preventDefault();
    const target = document.querySelector(this.getAttribute('href'));
    if(target){
      target.scrollIntoView({ behavior: 'smooth' });
    }
  });
});

// ===== Optional: Responsive Hamburger Menu =====
// Si tu ajoutes un bouton hamburger <button id="hamburger"></button> dans le header
const hamburger = document.getElementById('hamburger');
const navMenu = document.querySelector('header nav ul');

if(hamburger && navMenu){
  hamburger.addEventListener('click', () => {
    navMenu.classList.toggle('show');
  });
}

// Fermer le menu quand on clique sur un lien (utile pour mobile)
document.querySelectorAll('header nav ul li a').forEach(link => {
  link.addEventListener('click', () => {
    if(navMenu.classList.contains('show')){
      navMenu.classList.remove('show');
    }
  });
});
