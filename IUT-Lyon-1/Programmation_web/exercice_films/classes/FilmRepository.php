<?php
    Class FilmRepository extends Repository{

        /**
        *   constructor wich queries DB for films
        *   @return void
        */
        public function __construct(){
            $this->pdoConnect = connectDb();
            $query = $this->pdoConnect->prepare("select id_film,nom_film,annee_film,score from films");
            $query->execute();
            $films = $query->fetchAll();

            foreach($films as $film){
                $filmObject = new Film(
                    $film['nom_film'],
                    $film['annee_film'],
                    $film['score']
                );
                $filmObject->setId($film['id_film']);
                array_push($this->filmArray,$filmObject);
            }
        }


        /**
        *   add a film in the database
        *   @param film a film object
        *   @return int if the update was executed correctly or not
        *       => 0 if insert was executed
        *       => 1 if update was executed
        *       => 2 if an error happend
        */
        public function save($film){
			$testQuery = $this->pdoConnect->prepare("select count(*) as nb from films where nom_film=?");
			$testQuery->execute(array($film->getNom()));

			$data = $testQuery->fetch();

			if($data["nb"] > 0){
				$query = $this->pdoConnect->prepare("update films set annee_film =:annee,score=:score where lower(nom_film)=lower(:nom)");
				$insert = $query->execute(array(
					'nom' => $film->getNom(),
					'annee' => $film->getAnnee(),
					'score' => $film->getScore()
				));

                return ($insert ? 1 : 2);
			}else{
				$query = $this->pdoConnect->prepare("insert into films(nom_film,annee_film,score) values(:nom,:annee,:score)");
				$insert = $query->execute(array(
					'nom' => $film->getNom(),
					'annee' => $film->getAnnee(),
					'score' => $film->getScore()
				));

                return ($insert ? 0 : 2);
			}
        }

        /**
        *   get an item given an id
        *   @param $id the id
        *   @return $film an object of type Film
        */
        public function getFilmById($id){
            if(isset($id) && (int)$id >= 0){
                foreach($this->filmArray as $film){
                    if($film->getId() === (int)$id){
                        return $film;
                    }
                }
            }
        }

        /**
        *   get all films from DB
        *   @return a key=>value array containing all the films
        */
        public function getAll(){
            return $this->filmArray;
        }

        private $filmArray = array();
    }
